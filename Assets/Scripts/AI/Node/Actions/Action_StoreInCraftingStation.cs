namespace AI.Node.Jobs {
    public class Action_StoreInCraftingStation : BTNode {
        private readonly Settler _settler;

        public Action_StoreInCraftingStation(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            Settler_TransportForCrafting data = _settler.Data.craftingTransport;
            CraftingStation crafting = _settler.Data.craftingTransport.craftingStation;
            crafting.StoreResource(data.resourceInHands.ResourceType, data.resourceInHands.Amount);
            data.resourceInHands = ResourceData.Empty;
            data.resourceToHaul = null;
            data.craftingStation = null;
            data.amountToPick = 0;
            _settler.ResourceCarrier.DropResource();
            return BTNodeState.Success;
        }
    }
}