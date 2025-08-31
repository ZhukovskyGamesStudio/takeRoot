using UnityEngine;

namespace AI {
	public interface IZombieMover {
		public bool IsMoving { get; }
		public bool IsAtPosition(Vector2 target);
		public bool HasPath(Vector2 target);
		public void MoveTo(Vector2 target);
	}
}