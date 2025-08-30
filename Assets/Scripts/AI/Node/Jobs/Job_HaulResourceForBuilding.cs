using Unity.Networking.Transport;

namespace AI.Node.Jobs {
	public class Job_HaulResourceForBuilding : Sequence {
		
		public Job_HaulResourceForBuilding(Settler settler, IBuildingService buildingService, IResourceManager resourceManager) {
			SettlerData data = settler.Data;

			var clear = new Action_ClearHaulBuilding(settler, resourceManager);
			
			var move = new ConditionalAction()
				.Do(new Action_MoveToPos(settler))
				.While(() => data.buildingTransport.buildingBlueprint && data.buildingTransport.resourceToHaul);
			var moveToBlueprint = new ConditionalAction()
				.Do(new Action_MoveToPos(settler))
				.While(() => data.buildingTransport.buildingBlueprint);
			
			var pickUp = new Action_PickupResourceForBuilding(settler);


			var storeInBuildingBlueprint = new Action_StoreInBuildingBlueprint(settler);

			var transport = new Sequence()
				.AddChild(new Action_FindBuildingBlueprint(settler, buildingService))
				.AddChild(new Action_ReserveResourceForBuilding(settler, resourceManager))
				.AddChild(move)
				.AddChild(pickUp)
				.AddChild(moveToBlueprint)
				.AddChild(storeInBuildingBlueprint)
				.AddChild(clear);

			var job = new Selector()
				.AddChild(transport)
				.AddChild(clear);

			AddChild(job);
		}
		
	}
}