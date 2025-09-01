namespace AI.Node.Jobs {
    public class Action_SleepOnGround : BTNode {
        private Settler _settler;

        public Action_SleepOnGround(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            if (!_settler.Data.needs.Energy.isSleeping) {
                _settler.Sleep();
                _settler.Data.needs.Energy.energyChange = _settler.Data.needs.Energy.onGroundEnergyChange;
            }

            if (!_settler.Data.needs.Energy.IsTired) {
                _settler.Data.needs.Energy.energyChange = _settler.Data.needs.Energy.defaultEnergyChange;
                _settler.WakeUp();
                return _state = BTNodeState.Success;
            }

            return _state = BTNodeState.Running;
        }
    }
}