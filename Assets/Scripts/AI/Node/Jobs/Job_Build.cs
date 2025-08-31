namespace AI.Node.Jobs {
	public class Job_Build : Sequence {
		public Job_Build(Settler settler, IBuildingService buildingService) {
			var moveToBuilding = new ConditionalAction()
				.Do(new Action_MoveToPos(settler))
				.While(() => settler.Data.targets.BuildingBlueprint &&
				             settler.Data.targets.BuildingBlueprint.CanBuild());

			var build = new Sequence()
				.AddChild(new Action_FindBlueprintToBuild(settler, buildingService))
				.AddChild(moveToBuilding)
				.AddChild(new Action_Build(settler));

			var clear = new Action_ClearBuilding(settler);

			var job = new Selector()
				.AddChild(build)
				.AddChild(clear);
			
			AddChild(job);
		}
		
	}
}