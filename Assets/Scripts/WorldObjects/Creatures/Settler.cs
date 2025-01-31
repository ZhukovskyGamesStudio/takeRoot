using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Settler : ECSEntity {
    public const float CELL_SIZE = 1f;
    private static readonly int Mood = Animator.StringToHash("Mood");
    private static readonly int IsSleeping = Animator.StringToHash("IsSleeping");

    [SerializeField]
    private Mood _mood;

    [SerializeField]
    private Mode _mode;

    public Race Race;

    [SerializeField]
    private Animator _animator;

    [field: SerializeField]
    public GameObject ResourceHolder { get; private set; }

    private bool _isMoving;

    private Coroutine _performingCoroutine;

    public CommandData TakenCommand { get; private set; }
    public TacticalCommandData TakenTacticalCommand { get; private set; }
    public Mode Mode => _mode;

    public Vector2Int GetCellOnGrid => new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

    private void Update() {
        if (TakenCommand != null) {
            if (_performingCoroutine == null) {
                bool canPerform = CanPerform();

                if (canPerform) {
                    TryStartPerform(() => { CommandsManagersHolder.Instance.CommandsManager.PerformedCommand(TakenCommand); });
                } else {
                    Vector2Int? nextStepCell = TryMoveToCommandTarget();
                    if (nextStepCell == null) {
                        TakenCommand.UnablePerformSettlers.Add(this);
                        CommandsManagersHolder.Instance.CommandsManager.ReturnCommand(TakenCommand);
                    } else {
                        if (_performingCoroutine != null) {
                            return;
                        }

                        if (!_isMoving)
                            _performingCoroutine = StartCoroutine(MoveToSell(nextStepCell.Value));
                    }
                }
            }
        }

        if (TakenTacticalCommand != null) {
            if (_performingCoroutine == null) {
                if (TakenTacticalCommand.TacticalCommandType != TacticalCommand.Move) {
                    var canPerform = ExactInteractionChecker.CanInteract(GetCellOnGrid, TakenTacticalCommand.TacticalInteractable);
                    if (canPerform) {
                        TryStartPerform(() => {
                            CommandsManagersHolder.Instance.TacticalCommandsManager.PerformedCommand(TakenTacticalCommand);
                        });
                    } else {
                        Vector2Int? nextStepCell = TryMoveToTacticalCommandTarget();
                        if (nextStepCell == null) {
                            ClearTacticalCommand();
                        } else {
                            if (_performingCoroutine != null) return;
                            _performingCoroutine = StartCoroutine(MoveToSell(nextStepCell.Value));
                        }
                    }
                } else {
                    Vector2Int? nextStepCell = TryMoveToTacticalCommandTarget();
                    if (nextStepCell == null) {
                        ClearTacticalCommand();
                    } else {
                        if (_performingCoroutine != null) return;
                        if (!_isMoving)
                            _performingCoroutine = StartCoroutine(MoveToSell(nextStepCell.Value));
                    }
                }
            }
        }

        UpdateMoodAndAnimations();
    }

    private bool CanPerform() {
        if (TakenCommand.CommandType == Command.Store) {
            return TakenCommand.Additional && ExactInteractionChecker.CanInteractFromNeighborCell(GetCellOnGrid, TakenCommand.Additional);
        }

        return ExactInteractionChecker.CanInteractFromNeighborCell(GetCellOnGrid, TakenCommand.Interactable);
    }

    private void UpdateMoodAndAnimations() {
        //упростил пока-что, так более заметно
        if (_mode == Mode.Tactical) {
            _animator.SetBool(IsSleeping, false);
            _mood = global::Mood.Angry;
            _animator.SetInteger(Mood, (int)_mood);
            return;
        }

        if (TakenCommand != null && _performingCoroutine != null) {
            _animator.SetBool(IsSleeping, false);
            _mood = TakenCommand.CommandType switch {
                Command.Search => global::Mood.Happy,
                Command.Break => global::Mood.Angry,
                Command.Store => global::Mood.Neutral,
                Command.Transport => global::Mood.Sad,
                _ => _mood
            };
        } else if (TakenTacticalCommand != null && _performingCoroutine != null) {
            _animator.SetBool(IsSleeping, false);
            _mood = TakenTacticalCommand.TacticalCommandType switch {
                TacticalCommand.Move => global::Mood.Neutral,
                TacticalCommand.TacticalAttack => global::Mood.Angry,
                _ => _mood
            };
        } else {
            _animator.SetBool(IsSleeping, true);
            _mood = global::Mood.Neutral;
        }

        _animator.SetInteger(Mood, (int)_mood);
    }

    private Vector2Int? TryMoveToCommandTarget() {
        HashSet<Vector2Int> targetCell = new HashSet<Vector2Int>();

        if (TakenCommand.CommandType is Command.Transport) {
            targetCell.Add(TakenCommand.Interactable.GetInteractableCell);
        } else if (TakenCommand.CommandType == Command.Store) {
            Storagable st =
                ResourceManager.Instance.FindClosestAvailableStorage(TakenCommand.Interactable.GetComponent<ResourceView>().ResourceData,
                    GetCellOnGrid);
            if (st == null) {
                TakenCommand.Interactable.GetComponent<ResourceView>().DropOnGround();
                return null;
            }

            Interactable interactableStorage = st.GetComponent<Interactable>();
            TakenCommand.Additional = interactableStorage;
            targetCell.Add(interactableStorage.GetInteractableCell);
        } else {
            HashSet<Vector2Int> possibleTargets = TakenCommand.Interactable.InteractableCells;
            targetCell = possibleTargets.Where(AStarPathfinding.IsWalkable).ToHashSet();
        }

        Vector2Int? pathStep = ExactInteractionChecker.NextStepOnPath(GetCellOnGrid, targetCell);

        if (pathStep != null) {
            return pathStep;
        }

        //Debug.LogWarning($"Path is null! from:{GetCellOnGrid} to:{targetCell}");
        return null;
    }

    private Vector2Int? TryMoveToTacticalCommandTarget() {
        Vector2Int target;
        HashSet<Vector2Int> targetCell = new HashSet<Vector2Int>() { TakenTacticalCommand.TargetPosition };
        Vector2Int? pathStep = ExactInteractionChecker.NextStepOnPath(GetCellOnGrid, targetCell);
        if (pathStep != null) {
            target = pathStep.Value;
        } else {
            //Debug.LogWarning($"Path is null! from:{GetCellOnGrid} to:{targetCell}");
            return null;
        }

        return target;
    }

    private void MoveStep(Vector2Int direction) {
        transform.position += new Vector3(direction.x, direction.y) * CELL_SIZE;
        if (direction.x < 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        }

        if (direction.x > 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator MoveToSell(Vector2Int target) {
        Vector3 target3 = new Vector3(target.x, target.y);
        Vector3 diff = target3 - transform.position;
        //transform.position = target3 * CELL_SIZE;
        yield return StartCoroutine(LerpFromTo(transform.position, target3 * CELL_SIZE,
            ConfigManager.Instance.CreaturesParametersConfig.MoveTime));
        if (diff.x < 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        }

        if (diff.x > 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        _performingCoroutine = null;
    }

    private IEnumerator LerpFromTo(Vector3 from, Vector3 to, float time) {
        float elapsedTime = 0f;
        _isMoving = true;

        while (elapsedTime < time) {
            float t = elapsedTime / time;
            transform.position = Vector3.Lerp(from, to, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = to;
        _isMoving = false;
    }

    private void TryStartPerform(Action callback) {
        if (_performingCoroutine != null) {
            return;
        }

        _performingCoroutine = StartCoroutine(PerformingCoroutine(callback));
    }

    private IEnumerator PerformingCoroutine(Action after) {
        if (TakenCommand?.CommandType == Command.Break || TakenTacticalCommand?.TacticalCommandType == TacticalCommand.TacticalAttack) {
            Transform target = TakenCommand != null ? TakenCommand.Interactable.transform : TakenTacticalCommand.TacticalInteractable.transform;
            yield return StartCoroutine(Attack(ConfigManager.Instance.CreaturesParametersConfig.AttackTime, target, after));
        } else {
            yield return new WaitForSeconds(ConfigManager.Instance.CreaturesParametersConfig.PerformingTime);
            after?.Invoke();
        }

        _performingCoroutine = null;
    }

    private IEnumerator Attack(float delay, Transform target, Action callback) {
        Transform targetTransform = target.transform;
        Vector3 startPosition = transform.position;
        Vector3 direction = (targetTransform.position - startPosition).normalized;
        Vector3 targetPosition = startPosition + direction * ConfigManager.Instance.CreaturesParametersConfig.AttackAnimationShift;

        float elapsedTime = 0f;
        while (elapsedTime < delay / 2f) {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / (delay / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        callback?.Invoke();

        transform.position = targetPosition;
        elapsedTime = 0f;

        while (elapsedTime < delay / 2f) {
            transform.position = Vector3.Lerp(targetPosition, startPosition, elapsedTime / (delay / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;
    }

    public void SetCommand(CommandData data) {
        ClearCommand();
        TakenCommand = data;
        //if (CanPerform()) {
        //    return;
        //}

        //Vector2Int? nextStepCell = TryMoveToCommandTarget();
        //if (nextStepCell == null) {
        //    CommandsManagersHolder.Instance.CommandsManager.RevokeCommandBecauseItsUnreachable(TakenCommand);
        //}
    }

    public void ClearCommand() {
        TakenCommand = null;
        if (_performingCoroutine != null) {
            StopCoroutine(_performingCoroutine);
            _performingCoroutine = null;
        }
    }

    public void SetTacticalCommand(TacticalCommandData data) {
        ClearCommand();
        TakenTacticalCommand = data;
        Vector2Int? nextStepCell = TryMoveToTacticalCommandTarget();
        if (nextStepCell == null) {
            ClearTacticalCommand();
        }
    }

    public void ClearTacticalCommand() {
        TakenTacticalCommand = null;
        if (_performingCoroutine != null) {
            StopCoroutine(_performingCoroutine);
            _performingCoroutine = null;
        }
    }

    public void ChangeMode(Mode mode) {
        if (mode == Mode.Planning) {
            //TacticalCommandsManagersHolder.Instance.CommandsManager.SetActivePanel(false);
            //TacticalCommandsManagersHolder.Instance.CommandsManager.CancelCommand();
            if (TakenTacticalCommand != null) {
                TakenTacticalCommand = null;
            }
        }

        if (mode == Mode.Tactical) {
            //CommandsManagersHolder.Instance.CommandsManager.SetActivePanel(false);
            //TacticalCommandsManagersHolder.Instance.CommandsManager.SetActivePanel(true);
            if (TakenCommand != null) {
                CommandsManagersHolder.Instance.CommandsManager.RevokeCommandBecauseItsUnreachable(TakenCommand);
            }
        }

        _mode = mode;
    }
}

public enum Mood {
    Neutral = 0,
    Sad = 1,
    Angry = 2,
    Happy = 3
}

public enum Mode {
    Planning,
    Tactical
}