namespace AI.Node.Jobs {
	public class Action_TacticalAttack : BTNode {
		private readonly Settler _settler;

		public Action_TacticalAttack(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			var zombie = _settler.Data.tactical.Target;
			_settler.Attacker.Attack(zombie);
			return BTNodeState.Success;
		}
	}
}