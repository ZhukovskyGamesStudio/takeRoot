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
				_settler.Data.currTarget.CancelJob();
				damagable.Die();
				return _state = BTNodeState.Success;
			}
			_settler.Destroyer.Hit(damagable);
			return _state = BTNodeState.Running;
		}
	}
}