namespace AI.Node.Jobs {
    public class Action_FindCraftingStation : BTNode {
        private readonly Settler _settler;
        private readonly ICraftingService _craftingStation;

        public Action_FindCraftingStation(Settler settler, ICraftingService craftingStation) {
            _settler = settler;
            _craftingStation = craftingStation;
        }

        public override BTNodeState Evaluate() {
            if (_settler.Data.craftingTransport.craftingStation != null) {
                return BTNodeState.Success;
            }

            CraftingStation station = _craftingStation.GetCraftingStationWithJob();
            if (station == null) {
                return BTNodeState.Failure;
            }

            _settler.Data.craftingTransport.craftingStation = station;
            return BTNodeState.Success;
        }
    }
}