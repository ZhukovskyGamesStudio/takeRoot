namespace AI.Node.Jobs {
	public class Action_ClearTacticalData : BTNode {
		private readonly Settler _settler;

		public Action_ClearTacticalData(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			_settler.Data.tactical.HasTacticalMovePos = false;
			return BTNodeState.Failure;
		}
	}
}