using GameResources;

namespace AI.Node.Jobs {
    public class Action_PickupResource : BTNode {
        private readonly Settler _settler;

        public Action_PickupResource(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            Resource resource = _settler.Data.craftingTransport.resourceToHaul;
            resource.PickUp(_settler.Data.craftingTransport.amountToPick);
            _settler.Data.craftingTransport.resourceInHands = new ResourceData {
                ResourceType = resource.Type,
                Amount = _settler.Data.craftingTransport.amountToPick
            };
            _settler.ResourceCarrier.CarryResource(resource.Type);
            _settler.Data.curMovePos = _settler.Data.craftingTransport.craftingStation.transform.position;
            return BTNodeState.Success;
        }
    }
}