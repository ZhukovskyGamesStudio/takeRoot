using GameResources;

namespace AI.Node.Jobs {
	public class Action_PickupResourceForBuilding : BTNode {
		private readonly Settler _settler;

		public Action_PickupResourceForBuilding(Settler settler) {
			_settler = settler;
		}

		public override BTNodeState Evaluate() {
			Resource resource = _settler.Data.buildingTransport.resourceToHaul;
			resource.PickUp(_settler.Data.buildingTransport.amountToPick);
			_settler.Data.buildingTransport.resourceInHands = new ResourceData {
				ResourceType = resource.Type,
				Amount = _settler.Data.buildingTransport.amountToPick
			};
			_settler.ResourceCarrier.CarryResource(resource.Type);
			_settler.Data.curMovePos = _settler.Data.buildingTransport.buildingBlueprint.InteractionPos.position;
			return BTNodeState.Success;
		}
	}
}