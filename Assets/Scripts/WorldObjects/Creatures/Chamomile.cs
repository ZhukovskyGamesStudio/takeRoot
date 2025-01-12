using System;
using System.Collections;
using UnityEngine;

public class Chamomile : MonoBehaviour {
    private static readonly int Mood = Animator.StringToHash("Mood");
    private static readonly int IsSleeping = Animator.StringToHash("IsSleeping");

    public const float CELL_SIZE = 1f;

    [SerializeField]
    private Mood _mood;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _performingTime = 1, _moveTime = 0.2f;

    private Coroutine _performingCoroutine;

    public CommandData TakenCommand { get; private set; }

    public Vector2Int GetCellOnGrid => new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

    private void Update() {
        if (TakenCommand != null) {
            _animator.SetBool(IsSleeping, false);
            _mood = TakenCommand.CommandType switch {
                Command.Search => global::Mood.Happy,
                Command.Attack => global::Mood.Angry,
                Command.Store => global::Mood.Neutral,
                Command.Transport => global::Mood.Sad,
                _ => _mood
            };

            if (_performingCoroutine == null) {
                bool canPerfrom;
                if (TakenCommand.CommandType == Command.Store) {
                    canPerfrom = ExactInteractionChecker.CanInteract(GetCellOnGrid, TakenCommand.Additional);
                } else {
                    canPerfrom = ExactInteractionChecker.CanInteract(GetCellOnGrid, TakenCommand.Interactable);
                }

                if (canPerfrom) {
                    TryStartPerform(() => { CommandsManager.Instance.PerformedCommand(TakenCommand); });
                } else {
                    TryMoveToCommandTarget();
                }
            }
        } else {
            _animator.SetBool(IsSleeping, true);
            _mood = global::Mood.Neutral;
        }

        _animator.SetInteger(Mood, (int)_mood);
      
    }

    private void TryMoveToCommandTarget() {
        Vector2Int target = TakenCommand.Interactable.GetInteractableSell;
        Vector2Int targetCell = TakenCommand.Interactable.GetInteractableSell;
        if (TakenCommand.CommandType is Command.Search or Command.Attack or Command.Transport) {
            targetCell = TakenCommand.Interactable.GetInteractableSell;
        }

        if (TakenCommand.CommandType == Command.Store) {
            targetCell = TakenCommand.Additional.GetInteractableSell;
        }

        Vector2Int? pathStep = ExactInteractionChecker.NextStepOnPath(GetCellOnGrid, targetCell);

        if (pathStep != null) {
            target = pathStep.Value;
        } else {
            Debug.LogWarning($"Path is null! from:{GetCellOnGrid} to:{targetCell}");
        }

        TryStartPerform(() => { MoveToSell(target); });
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

    private void MoveToSell(Vector2Int target) {
        Vector3 target3 = new Vector3(target.x, target.y);
        Vector3 diff = target3 - transform.position;
        //transform.position = target3 * CELL_SIZE;
        StartCoroutine(LerpFromTo(transform.position, target3 * CELL_SIZE, _moveTime));
        if (diff.x < 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        }

        if (diff.x > 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator LerpFromTo(Vector3 from, Vector3 to, float time) {
        float elapsedTime = 0f;

        while (elapsedTime < time) {
            float t = elapsedTime / time;
            transform.position = Vector3.Lerp(from, to, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = to;
    }

    private void TryStartPerform(Action callback) {
        if (_performingCoroutine != null) {
            return;
        }

        _performingCoroutine = StartCoroutine(PerformingCoroutine(callback));
    }

    private IEnumerator PerformingCoroutine(Action after) {
        yield return new WaitForSeconds(_performingTime);
        after?.Invoke();
        _performingCoroutine = null;
    }

    public void SetCommand(CommandData data) {
        ClearCommand();
        TakenCommand = data;
    }

    public void ClearCommand() {
        TakenCommand = null;
        if (_performingCoroutine != null) {
            StopCoroutine(_performingCoroutine);
            _performingCoroutine = null;
        }
    }
}

public enum Mood {
    Neutral = 0,
    Sad = 1,
    Angry = 2,
    Happy = 3
}