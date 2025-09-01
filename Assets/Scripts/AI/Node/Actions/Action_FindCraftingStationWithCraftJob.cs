namespace AI.Node.Jobs {
    public class Action_FindCraftingStationWithCraftJob : BTNode {
        private readonly Settler _settler;
        private readonly ICraftingService _craftingService;

        public Action_FindCraftingStationWithCraftJob(Settler settler, ICraftingService craftingService) {
            _settler = settler;
            _craftingService = craftingService;
        }

        public override BTNodeState Evaluate() {
            var race = _settler.Data.names.Race;
            var craftingStation = _craftingService.GetCraftingStationWithAvailableCrafting(race);
            if (craftingStation == null) {
                return _state = BTNodeState.Failure;
            }

            _settler.Data.targets.CraftingStation = craftingStation;
            craftingStation.Crafters[race] = _settler;
            _settler.Data.curMovePos = craftingStation.InteractPos[race == Race.Plants ? 1 : 0].position;
            return _state = BTNodeState.Success;
        }
    }
}