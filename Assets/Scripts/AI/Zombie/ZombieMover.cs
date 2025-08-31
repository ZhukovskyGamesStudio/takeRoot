using UnityEngine;

namespace AI {
	public class ZombieMover : MonoBehaviour, IZombieMover {
		public float moveTime = 1f;
		public float gridSize = 1f;
		
		[SerializeField]
		private bool RotateWhileMove = true;

		public bool IsMoving { get; private set; }

		public bool IsAtPosition(Vector2 target) {
			return false;
		}

		public bool HasPath(Vector2 target) {
			return true;
		}

		public void MoveTo(Vector2 target) {
			Debug.Log("Moving");
		}
	}
}