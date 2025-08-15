using UnityEngine;

namespace AI.Node.Jobs {
	public class Action_MoveTo : BTNode {
		private Settler _settler;
		private readonly bool _useSubsequentTarget;

		public Action_MoveTo(Settler settler, bool useSubsequentTarget = false) {
			_settler = settler;
			_useSubsequentTarget = useSubsequentTarget;
		}
		public override BTNodeState Evaluate() {
			var target = _useSubsequentTarget ? _settler.Data.subsequentTarget : _settler.Data.currTarget;
			if (!_settler.Mover.HasPath(target.InteractPosition.position)) {
				return BTNodeState.Failure;
			}
			if (_settler.Mover.IsAtPosition(target.InteractPosition.position)) {
				return BTNodeState.Success;
			}
			_settler.Mover.MoveTo(target.InteractPosition.position, _settler.WorkerAnimator);
			return BTNodeState.Running;
		}
	}
}