namespace AI.Node.Jobs {
    public class Action_Plant : BTNode {
        private readonly Settler _settler;

        public Action_Plant(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            CommandTarget target = _settler.Data.currTarget;
            var plot = target.GetComponent<FarmingPlot>();
            if (plot.PlantState != FarmingPlantState.WaitingForPlanting) {
                _settler.Planter.Cancel();
                plot.Farmer = null;
                return _state = BTNodeState.Success;
            }

            _settler.Planter.Plant(plot);
            return _state = BTNodeState.Running;
        }
    }
}