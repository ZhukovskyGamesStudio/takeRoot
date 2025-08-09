using UnityEngine;

namespace AI.Node.Jobs {
	public class Action_MoveTo : BTNode {
		private Settler _settler;

		public Action_MoveTo(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			if (!_settler.Mover.HasPath(_settler.Data.currTarget.InteractPosition.position)) {
				return BTNodeState.Failure;
			}
			if (_settler.Mover.IsAtPosition(_settler.Data.currTarget.InteractPosition.position)) {
				return BTNodeState.Success;
			}
			_settler.Mover.MoveTo(_settler.Data.currTarget.InteractPosition.position, _settler.WorkerAnimator);
			return BTNodeState.Running;
		}
	}
}