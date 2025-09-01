using GameResources;

namespace AI.Node.Jobs {
    public class Action_PickupResourceForCrafting : BTNode {
        private readonly Settler _settler;

        public Action_PickupResourceForCrafting(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            Resource resource = _settler.Data.craftingTransport.resourceToHaul;
            if (_settler.Data.craftingTransport.craftingStation.HaulInteractPos == null) {
                return BTNodeState.Failure;
            }
            resource.PickUp(_settler.Data.craftingTransport.amountToPick);
            _settler.Data.craftingTransport.resourceInHands = new ResourceData {
                ResourceType = resource.Type,
                Amount = _settler.Data.craftingTransport.amountToPick
            };
            _settler.ResourceCarrier.CarryResource(resource.Type);
            _settler.Data.curMovePos = _settler.Data.craftingTransport.craftingStation.HaulInteractPos.Value;
            return BTNodeState.Success;
        }
    }
}