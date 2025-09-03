using UnityEngine;

namespace AI.Node.Jobs {
	public class Action_TacticalMove : BTNode {
		private readonly Settler _settler;

		public Action_TacticalMove(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			Vector3 pos = _settler.Data.tactical.TacticalMovePos;
			if (!_settler.Mover.HasPath(pos)) {
				return _state = BTNodeState.Failure;
			}

			if (_settler.Mover.IsAtPosition(pos)) {
				return _state = BTNodeState.Success;
			}

			_settler.Mover.MoveTo(pos);
			return _state = BTNodeState.Running;
		}
	}
}