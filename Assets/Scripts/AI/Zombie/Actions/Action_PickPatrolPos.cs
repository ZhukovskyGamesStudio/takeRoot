using AI.Node;
using UnityEngine;

namespace AI {
	public class Action_PickPatrolPos : BTNode {
		private readonly Zombie _zombie;
		private readonly int _range = 2;

		public Action_PickPatrolPos(Zombie zombie) {
			_zombie = zombie;
		}
		public override BTNodeState Evaluate() {
			var data = _zombie.Data;
			data.PatrolTimer += Time.deltaTime;
			if (data.PatrolTimer >= data.PatrolCooldown)
				return BTNodeState.Failure;
			Vector3 offset = new(Random.Range(-_range, _range), Random.Range(-_range, _range));
			var pos = _zombie.transform.position + offset;
			if (_zombie.Mover.HasPath(pos)) {
				data.CurrMovePos = pos;
				return _state = BTNodeState.Success;
			}
			return _state = BTNodeState.Failure;
		}
	}
}