namespace AI.Node.Jobs {
	public class Action_ClearTacticalData : BTNode {
		private readonly Settler _settler;

		public Action_ClearTacticalData(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			var dataTactical = _settler.Data.tactical;
			dataTactical.HasTacticalMovePos = false;
			return BTNodeState.Failure;
		}
	}
}