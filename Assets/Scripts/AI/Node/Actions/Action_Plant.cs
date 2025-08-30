namespace AI.Node.Jobs {
    public class Action_Plant : BTNode {
        private readonly Settler _settler;

        public Action_Plant(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            var plot = _settler.Data.farming.FarmingPlot;
            if (plot.PlantState != FarmingPlantState.WaitingForPlanting) {
                _settler.Farmer.Cancel();
                plot.Farmer = null;
                return _state = BTNodeState.Success;
            }

            _settler.Farmer.Plant(plot);
            return _state = BTNodeState.Running;
        }
    }
}