using UnityEngine;

namespace AI.Node.Jobs {
	public class Action_ClearMovePos : BTNode {
		private readonly Settler _settler;

		public Action_ClearMovePos(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			_settler.Data.HasMovePos = false;
			_settler.Data.IdleMoveTimer = 0;
			return _state = BTNodeState.Success;
		}
	}
}