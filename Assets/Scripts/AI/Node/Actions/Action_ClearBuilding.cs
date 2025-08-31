namespace AI.Node.Jobs {
	public class Action_ClearBuilding : BTNode{
		private readonly Settler _settler;

		public Action_ClearBuilding(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			var buildingData = _settler.Data.building;
			if (buildingData.buildingBlueprint != null) {
				buildingData.buildingBlueprint.Builder = null;
			}
			buildingData.buildingBlueprint = null;
			return BTNodeState.Failure;
		}
	}
}