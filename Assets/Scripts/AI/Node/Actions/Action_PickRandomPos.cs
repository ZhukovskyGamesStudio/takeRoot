using UnityEngine;

namespace AI.Node.Jobs {
	public class Action_PickRandomPos : BTNode {
		private readonly Settler _settler;
		private int _range = 1;
		
		public Action_PickRandomPos(Settler settler) {
			_settler = settler;
		}

		public override BTNodeState Evaluate() {
			var pos = new Vector3(Random.Range(-_range, _range), Random.Range(-_range, _range));
			if (_settler.Mover.HasPath(pos)) {
				_settler.Data.curMovePos = pos;
				_settler.Data.HasMovePos = true;
				return _state = BTNodeState.Success;
			}
			return _state = BTNodeState.Failure;
		}
	}
}