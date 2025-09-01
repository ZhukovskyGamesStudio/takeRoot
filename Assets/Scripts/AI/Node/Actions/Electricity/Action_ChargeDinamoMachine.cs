namespace AI.Node.Jobs {
    public class Action_ChargeDinamoMachine : BTNode {
        private readonly Settler _settler;

        public Action_ChargeDinamoMachine(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            var dinamo = _settler.Data.targets.DinamoMachine;
            if (dinamo.HighElectricity) {
                _settler.DinamoCharger.Cancel();
                dinamo.DinamoCharger = null;
                return _state = BTNodeState.Success;
            }

            _settler.DinamoCharger.Charge(dinamo);
            return _state = BTNodeState.Running;
        }
    }
}