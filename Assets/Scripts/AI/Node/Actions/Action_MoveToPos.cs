using UnityEngine;

namespace AI.Node.Jobs {
	public class Action_MoveToPos : BTNode {
		private Settler _settler;
		private readonly bool _useSubsequentTarget;

		public Action_MoveToPos(Settler settler, bool useSubsequentTarget = false) {
			_settler = settler;
			_useSubsequentTarget = useSubsequentTarget;
		}
		public override BTNodeState Evaluate() {
			var pos = _settler.Data.curMovePos;
			if (!_settler.Mover.HasPath(pos)) {
				return BTNodeState.Failure;
			}
			if (_settler.Mover.IsAtPosition(pos)) {
				return BTNodeState.Success;
			}
			_settler.Mover.MoveTo(pos, _settler.WorkerAnimator);
			return BTNodeState.Running;
		}
	}
}