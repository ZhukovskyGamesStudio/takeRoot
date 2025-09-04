namespace AI.Node.Jobs {
    public class Action_SleepOnGround : BTNode {
        private Settler _settler;

        public Action_SleepOnGround(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            if (!_settler.Data.needs.Value.Energy.isSleeping) {
                _settler.Sleep();
                _settler.Data.needs.Value.Energy.energyChange = _settler.Data.needs.Value.Energy.onGroundEnergyChange;
            }

            if (!_settler.Data.needs.Value.Energy.IsTired) {
                _settler.Data.needs.Value.Energy.energyChange = _settler.Data.needs.Value.Energy.defaultEnergyChange;
                _settler.WakeUp();
                return _state = BTNodeState.Success;
            }

            return _state = BTNodeState.Running;
        }
    }
}