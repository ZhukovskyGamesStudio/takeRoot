namespace AI.Node.Jobs {
	public class Action_FindBlueprintToBuild : BTNode{
		private readonly Settler _settler;
		private readonly IBuildingService _buildingService;

		public Action_FindBlueprintToBuild(Settler settler, IBuildingService buildingService) {
			_settler = settler;
			_buildingService = buildingService;
		}

		public override BTNodeState Evaluate() {
			var buildingBlueprint = _buildingService.GetBuildingBlueprintWithBuildJob();
			if (buildingBlueprint == null) {
				return BTNodeState.Failure;
			}
			_settler.Data.building.buildingBlueprint = buildingBlueprint;
			buildingBlueprint.Builder = _settler;
			_settler.Data.curMovePos = buildingBlueprint.InteractionPos.position;
			return _state = BTNodeState.Success;
		}
	}
}