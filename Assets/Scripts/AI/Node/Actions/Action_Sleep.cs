namespace AI.Node.Jobs {
    public class Action_Sleep : BTNode {
        private Settler _settler;

        public Action_Sleep(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            if (!_settler.Data.energy.isSleeping) {
                _settler.transform.position = _settler.Data.energy.bed.SleepPos.position;
                _settler.Data.energy.isSleeping = true;
                _settler.Data.energy.energyChange = _settler.Data.energy.onBedEnergyChange;
                _settler.Sleep();
            }

            if (!_settler.Data.energy.IsTired) {
                _settler.Data.energy.energyChange = _settler.Data.energy.defaultEnergyChange;
                _settler.transform.position = _settler.Data.energy.bed.NearPos.position;
                _settler.WakeUp();
                return _state = BTNodeState.Success;
            }

            return _state = BTNodeState.Running;
        }
    }
}