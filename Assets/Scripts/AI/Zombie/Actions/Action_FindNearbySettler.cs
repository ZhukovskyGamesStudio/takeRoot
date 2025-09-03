using AI.Node;
using UnityEngine;

namespace AI {
	public class Action_FindNearbySettler : BTNode {
		private readonly Zombie _zombie;
		private readonly ISettlersService _settlers;

		public Action_FindNearbySettler(Zombie zombie, ISettlersService settlers) {
			_zombie = zombie;
			_settlers = settlers;
		}
		public override BTNodeState Evaluate() {
			var pos = _zombie.transform.position;
			var detectRange = _zombie.Data.DetectRange;
			var x = (int)pos.x - detectRange / 2;
			var y = (int)pos.y - detectRange / 2;
			var detectArea = new Rect(x, y, detectRange, detectRange);
			var settler = _settlers.GetSettlerInArea(detectArea);
			_zombie.Data.DetectArea = detectArea;
			if (settler != null) {
				if (!_zombie.Data.Target)
					_zombie.Data.Target = settler;
				return BTNodeState.Success;
			}
			_zombie.Data.Target = null;
			return BTNodeState.Failure;
		}
	}
}