namespace AI.Node.Jobs {
    public class Action_ClearDinamoMachine : BTNode {
        private readonly Settler _settler;

        public Action_ClearDinamoMachine(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            _settler.Data.targets.DinamoMachine.DinamoCharger = null;
            _settler.Data.targets.DinamoMachine = null;
            return _state = BTNodeState.Failure;
        }
    }
}