namespace AI.Node.Jobs {
    public class Action_Sleep : BTNode {
        private Settler _settler;

        public Action_Sleep(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            if (!_settler.Data.needs.Energy.isSleeping) {
                _settler.transform.position = _settler.Data.needs.Energy.bed.SleepPos.position;
                _settler.Data.needs.Energy.isSleeping = true;
                _settler.Data.needs.Energy.energyChange = _settler.Data.needs.Energy.onBedEnergyChange;
                _settler.Sleep();
            }

            if (!_settler.Data.needs.Energy.IsTired) {
                _settler.Data.needs.Energy.energyChange = _settler.Data.needs.Energy.defaultEnergyChange;
                _settler.transform.position = _settler.Data.needs.Energy.bed.NearPos.position;
                _settler.WakeUp();
                _settler.Data.needs.Energy.bed.ReleaseBed();
                return _state = BTNodeState.Success;
            }

            return _state = BTNodeState.Running;
        }
    }
}