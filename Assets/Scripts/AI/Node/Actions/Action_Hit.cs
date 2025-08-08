namespace AI.Node.Jobs {
	public class Action_Hit : BTNode {
		private Settler _settler;

		public Action_Hit(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			var damagable = _settler.Data.currTarget;
			if (damagable.IsDead) {
				_settler.Destroyer.Cancel();
				damagable.Die();
				return BTNodeState.Success;
			}
			_settler.Destroyer.Hit(damagable);
			return BTNodeState.Running;
		}
	}
}