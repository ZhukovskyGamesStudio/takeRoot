using UnityEngine;

namespace AI.Node.Jobs {
/*
	public class Action_MoveToBlueprint : BTNode{
		private readonly Settler _settler;

		public Action_MoveToBlueprint(Settler settler) {
			_settler = settler;
		}

		public override BTNodeState Evaluate() {
			var blueprint = _settler.Data.buildingTransport.buildingBlueprint;
			if (blueprint == null) {
				return BTNodeState.Failure;
			}
			Vector3 pos = blueprint.InteractionPos.position;
			if (!_settler.Mover.HasPath(pos)) {
				return _state = BTNodeState.Failure;
			}
			if (_settler.Mover.IsAtPosition(pos)) {
				return _state = BTNodeState.Success;
			}
			_settler.Mover.MoveTo(pos, _settler.WorkerAnimator);
			return _state = BTNodeState.Running;
		}
	}
*/
}