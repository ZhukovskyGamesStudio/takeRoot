namespace AI.Node.Jobs {
	public class Job_Build : Sequence {

		public Job_Build(Settler settler, IBuildingService buildingService) {
			AddChild(new Action_FindBlueprintToBuild(settler, buildingService));
			
			var moveToBuilding = new ConditionalAction()
				.Do(new Action_MoveToPos(settler))
				.While(() => settler.Data.building.buildingBlueprint &&
				             settler.Data.building.buildingBlueprint.CanBuild());

			var build = new Action_Build(settler);
			
			AddChild(moveToBuilding);
			AddChild(build);
		}
		
	}
}