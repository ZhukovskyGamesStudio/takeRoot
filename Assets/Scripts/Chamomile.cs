using System;
using System.Collections;
using UnityEngine;

public class Chamomile : MonoBehaviour {
    private static readonly int Mood = Animator.StringToHash("Mood");

    public const float CELL_SIZE = 1f;

    [SerializeField]
    private Mood _mood;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _performingTime = 1;

    private Coroutine _performingCoroutine;

    public CommandData TakenCommand { get; private set; }

    public Vector2Int GetCellOnGrid => new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

    private void Update() {
        if (TakenCommand != null) {
            _mood = global::Mood.Neutral;

            if (_performingCoroutine == null) {
                if ((TakenCommand.InteractableObject.GetCellOnGrid - GetCellOnGrid).magnitude < 2) {
                    TryStartPerform(() => { CommandsManager.Instance.PerformedCommand(TakenCommand); });
                } else {
                    TryMoveToCommandTarget();
                }
            }
        } else {
            _mood = global::Mood.Sad;
        }

        _animator.SetInteger(Mood, (int)_mood);
    }

    private void TryMoveToCommandTarget() {
        Vector2Int target = TakenCommand.InteractableObject.GetCellOnGrid;
        Vector2Int direction = target - GetCellOnGrid;
        if (direction.x != 0) {
            direction.x = Mathf.RoundToInt(Mathf.Sign(direction.x));
        }

        if (direction.y != 0) {
            direction.y = Mathf.RoundToInt(Mathf.Sign(direction.y));
        }

        TryStartPerform(() => { MoveStep(direction); });
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
        TakenCommand = data;
    }

    public void ClearCommand() {
        TakenCommand = null;
        if (_performingCoroutine != null) {
            StopCoroutine(_performingCoroutine);
        }
    }
}

public enum Mood {
    Neutral = 0,
    Sad = 1
}