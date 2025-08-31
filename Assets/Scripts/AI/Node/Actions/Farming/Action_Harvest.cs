namespace AI.Node.Jobs {
    public class Action_Harvest : BTNode {
        private readonly Settler _settler;

        public Action_Harvest(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            var plot = _settler.Data.targets.FarmingPlot;
            if (plot.PlantState != FarmingPlantState.ReadyToHarvest) {
                _settler.Farmer.Cancel();
                plot.Farmer = null;
                return _state = BTNodeState.Success;
            }

            _settler.Farmer.Harvest(plot);
            return _state = BTNodeState.Running;
        }
    }
}