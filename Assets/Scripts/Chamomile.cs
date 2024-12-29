using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chamomile : MonoBehaviour {
   private static readonly int Mood = Animator.StringToHash("Mood");

   public const float CELL_SIZE = 0.35f;
   [SerializeField]
   private Mood _mood;

   [SerializeField]
   private Animator _animator;

   [SerializeField]
   private float _performingTime = 1;

   private Coroutine _performingCoroutine;

   private void Update() {
      if (Input.GetKeyDown(KeyCode.W)) {
         Move(new Vector2Int(0, 1));
      } else if (Input.GetKeyDown(KeyCode.A)) {
         Move(new Vector2Int(-1, 0));
      } else if (Input.GetKeyDown(KeyCode.S)) {
         Move(new Vector2Int(0, -1));
      } else if (Input.GetKeyDown(KeyCode.D)) {
         Move(new Vector2Int(1, 0));
      }

      if (Input.GetKeyDown(KeyCode.E)) {
         _mood = (Mood)(((int)_mood + 1) % 2);
      }

      _animator.SetInteger(Mood, (int)_mood);
   }

   private void Move(Vector2Int direction) {
      TryStartPerform(() => { MoveStep(direction);});
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
         StopCoroutine(_performingCoroutine);
      }

      _performingCoroutine = StartCoroutine(PerformingCoroutine(callback));
   }
   private IEnumerator PerformingCoroutine(Action after) {
      yield return new WaitForSeconds(_performingTime);
      after?.Invoke();
   }
}

public enum Mood {
   Neutral = 0,
   Sad = 1
}
