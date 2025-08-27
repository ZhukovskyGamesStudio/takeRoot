namespace AI.Node.Jobs {
	public class Action_StoreInBuildingBlueprint : BTNode {
		private readonly Settler _settler;

		public Action_StoreInBuildingBlueprint(Settler settler) {
			_settler = settler;
		}

		public override BTNodeState Evaluate() {
			Settler_TransportForBuilding data = _settler.Data.buildingTransport;
			BuildingBlueprint blueprint = _settler.Data.buildingTransport.buildingBlueprint;
			blueprint.StoreResource(data.resourceInHands.ResourceType, data.resourceInHands.Amount);
			data.resourceInHands = ResourceData.Empty;
			data.resourceToHaul = null;
			data.buildingBlueprint = null;
			data.amountToPick = 0;
			_settler.ResourceCarrier.DropResource();
			return BTNodeState.Success;
		}
	}
}