using UnityEngine;

namespace AI.Node.Jobs {
	public class Action_PickRandomPos : BTNode {
		private readonly Settler _settler;
		private int _range = 5;
		
		public Action_PickRandomPos(Settler settler) {
			_settler = settler;
		}
		
		public override BTNodeState Evaluate() {
			var pos = new Vector3(Random.Range(-_range, _range), 0, Random.Range(-_range, _range));
			_settler.Data.curMovePos = pos;
			_settler.Data.HasMovePos = true;
			return BTNodeState.Success;
		}
	}
}