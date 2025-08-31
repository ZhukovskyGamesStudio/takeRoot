using AI.Node;

namespace AI {
	public class Action_ZombieMove : BTNode{
		private readonly Zombie _zombie;

		public Action_ZombieMove(Zombie zombie) {
			_zombie = zombie;
		}
		public override BTNodeState Evaluate() {
			var pos = _zombie.Data.CurrMovePos;
			if (_zombie.Mover.IsAtPosition(pos)) {
				return BTNodeState.Success;
			}
			if (!_zombie.Mover.HasPath(pos)) {
				return BTNodeState.Failure;
			}
			_zombie.Mover.MoveTo(pos);
			return BTNodeState.Running;
		}
	}
}