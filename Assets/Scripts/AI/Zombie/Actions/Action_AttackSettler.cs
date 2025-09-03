using AI.Node;

namespace AI {
	public class Action_AttackSettler : BTNode {
		private readonly Zombie _zombie;

		public Action_AttackSettler(Zombie zombie) {
			_zombie = zombie;
		}
		public override BTNodeState Evaluate() {
			_zombie.Attacker.Attack(_zombie.Data.Target);
			return BTNodeState.Success;
		}
	}
}