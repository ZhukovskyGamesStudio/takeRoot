namespace AI.Node.Jobs {
    public class Action_ClearFarmingPlotFromFarmer : BTNode {
        private readonly Settler _settler;

        public Action_ClearFarmingPlotFromFarmer(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            _settler.Data.targets.FarmingPlot.Farmer = null;
            _settler.Data.targets.FarmingPlot = null;
            return _state = BTNodeState.Failure;
        }
    }
}