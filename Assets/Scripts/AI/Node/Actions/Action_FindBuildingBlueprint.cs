namespace AI.Node.Jobs {
	public class Action_FindBuildingBlueprint : BTNode {
		private readonly Settler _settler;
		private readonly IBuildingService _buildingService;

		public Action_FindBuildingBlueprint(Settler settler, IBuildingService buildingService) {
			_settler = settler;
			_buildingService = buildingService;
		}
		public override BTNodeState Evaluate() {
			var buildingData = _settler.Data.buildingTransport;
			if (buildingData.buildingBlueprint != null) {
				return BTNodeState.Success;
			}
			BuildingBlueprint buildingBlueprint = _buildingService.GetBuildingBlueprintWithTransportJob();
			if (buildingBlueprint == null || !_settler.Mover.HasPath(buildingBlueprint.InteractionPos.position)) {
				return BTNodeState.Failure;
			}
			
			buildingData.buildingBlueprint = buildingBlueprint;
			return BTNodeState.Success;
		}
	}
}