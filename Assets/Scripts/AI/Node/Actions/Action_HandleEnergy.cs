using UnityEngine;

namespace AI.Node.Jobs {
	public class Action_HandleEnergy : BTNode {
		private readonly Settler _settler;

		public Action_HandleEnergy(Settler settler) {
			_settler = settler;
		}

		public override BTNodeState Evaluate() {
			_settler.Data.energyTimer += Time.deltaTime;
			if (_settler.Data.energyTimer >= _settler.Data.energyCooldown) {
				_settler.Data.currentEnergy += _settler.Data.energyChange;
				_settler.Data.energyTimer = 0;
			}
			return _state = BTNodeState.Success;
		}
	}
}