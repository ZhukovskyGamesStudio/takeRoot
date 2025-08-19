namespace AI.Node.Jobs {
	public class Action_Water : BTNode {
		private readonly Settler _settler;

		public Action_Water(Settler settler) {
			_settler = settler;
		}

		public override BTNodeState Evaluate() {
			var target = _settler.Data.currTarget;
			if (target.EnoughWater) {
				_settler.Waterer.Cancel();
				_settler.Data.currTarget.CancelJob();
				return _state = BTNodeState.Success;
			}
			_settler.Waterer.Water(target);
			return _state = BTNodeState.Running;
		}
	}
}