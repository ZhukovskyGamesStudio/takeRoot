using Unity.Networking.Transport;

namespace AI.Node.Jobs {
	public class Job_HaulResourceForBuilding : Sequence {
		
		public Job_HaulResourceForBuilding(Settler settler, IBuildingService buildingService, IResourceManager resourceManager) {
			SettlerData data = settler.Data;
			AddChild(new Action_FindBuildingBlueprint(settler, buildingService));
			AddChild(new Action_ReserveResourceForBuilding(settler, resourceManager));
			ConditionalAction move = new ConditionalAction().Do(new Action_MoveToPos(settler)).While(() => data.buildingTransport.buildingBlueprint && data.buildingTransport.resourceToHaul);
			Selector pickUp = new Selector().AddChild(new Conditional(() => data.craftingTransport.HasResourceInHands))
				.AddChild(new Action_PickupResourceForBuilding(settler));

			ConditionalAction moveToBlueprint = new ConditionalAction().Do(new Action_MoveToPos(settler)).While(() => data.buildingTransport.buildingBlueprint);

			Action_StoreInBuildingBlueprint storeInBuildingBlueprint = new(settler);
			AddChild(move);
			AddChild(pickUp);
			AddChild(moveToBlueprint);
			AddChild(storeInBuildingBlueprint);

		}
		
	}
}