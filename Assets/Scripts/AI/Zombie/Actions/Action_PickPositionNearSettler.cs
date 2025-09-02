using AI.Node;

namespace AI {
	public class Action_PickPositionNearSettler : BTNode {
		private readonly Zombie _zombie;

		public Action_PickPositionNearSettler(Zombie zombie) {
			_zombie = zombie;
		}
		public override BTNodeState Evaluate() {
			var pos = _zombie.Data.Target.transform.position;
			if (_zombie.Data.CurrMovePos != pos)
				_zombie.Data.CurrMovePos = pos;
			return BTNodeState.Success;
		}
	}
}