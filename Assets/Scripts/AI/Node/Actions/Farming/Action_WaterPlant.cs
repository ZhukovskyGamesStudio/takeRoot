namespace AI.Node.Jobs {
    public class Action_WaterPlant : BTNode {
        private readonly Settler _settler;

        public Action_WaterPlant(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            var plot = _settler.Data.farming.FarmingPlot;
            if (plot.PlantState != FarmingPlantState.WaitingForWater) {
                _settler.Farmer.Cancel();
                plot.Farmer = null;
                return _state = BTNodeState.Success;
            }

            _settler.Waterer.Water(plot.GetComponent<CommandTarget>());
            return _state = BTNodeState.Running;
        }
    }
}