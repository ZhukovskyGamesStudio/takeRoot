namespace AI.Node.Jobs {
    public class Action_SleepOnGround : BTNode {
        private Settler _settler;

        public Action_SleepOnGround(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            if (!_settler.Data.energy.isSleeping) {
                _settler.Sleep();
                _settler.Data.energy.energyChange = _settler.Data.energy.onGroundEnergyChange;
            }

            if (!_settler.Data.energy.IsTired) {
                _settler.Data.energy.energyChange = _settler.Data.energy.defaultEnergyChange;
                _settler.WakeUp();
                return _state = BTNodeState.Success;
            }

            return _state = BTNodeState.Running;
        }
    }
}