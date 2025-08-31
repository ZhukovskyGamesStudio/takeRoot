using UnityEngine;

namespace Settlers.AI.Zombie {
	public class ZombieMover : IZombieMover {
		public float moveTime = 1f;
		public float gridSize = 1f;
		
		[SerializeField]
		private bool RotateWhileMove = true;

		public bool IsMoving { get; private set; }

		public bool IsAtPosition(Vector2 target) {
			throw new System.NotImplementedException();
		}

		public bool HasPath(Vector2 target) {
			throw new System.NotImplementedException();
		}

		public void MoveTo(Vector2 target) {
			throw new System.NotImplementedException();
		}
	}
}