namespace AI.Node.Jobs {
	public class Action_ClearBuilding : BTNode{
		private readonly Settler _settler;

		public Action_ClearBuilding(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			var targetsData = _settler.Data.targets;
			if (targetsData.BuildingBlueprint != null) {
				targetsData.BuildingBlueprint.Builder = null;
			}
			targetsData.BuildingBlueprint = null;
			return BTNodeState.Failure;
		}
	}
}