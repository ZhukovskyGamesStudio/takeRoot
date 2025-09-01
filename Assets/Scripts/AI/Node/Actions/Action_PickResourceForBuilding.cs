using GameResources;

namespace AI.Node.Jobs {
	public class Action_PickupResourceForBuilding : BTNode {
		private readonly Settler _settler;

		public Action_PickupResourceForBuilding(Settler settler) {
			_settler = settler;
		}

		public override BTNodeState Evaluate() {
			var dataBuildingTransport = _settler.Data.buildingTransport;
			if (dataBuildingTransport.resourceInHands.ResourceType != ResourceType.None) {
				return BTNodeState.Success;
			}
			Resource resource = dataBuildingTransport.resourceToHaul;
			resource.PickUp(dataBuildingTransport.amountToPick);
			dataBuildingTransport.resourceInHands = new ResourceData {
				ResourceType = resource.Type,
				Amount = dataBuildingTransport.amountToPick
			};
			if (dataBuildingTransport.buildingBlueprint.InteractionPos == null) {
				return BTNodeState.Failure;
			}
			_settler.ResourceCarrier.CarryResource(resource.Type);
			_settler.Data.curMovePos = dataBuildingTransport.buildingBlueprint.InteractionPos.Value;
			return BTNodeState.Success;
		}
	}
}