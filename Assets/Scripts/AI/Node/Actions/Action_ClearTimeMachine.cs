namespace AI.Node.Jobs {
    public class Action_ClearTimeMachine : BTNode {
        private readonly Settler _settler;

        public Action_ClearTimeMachine(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            _settler.TimeMachineCharger.Cancel();

            TimeMachine timeMachine = _settler.Data.targets.TimeMachine;
            if (timeMachine) {
                timeMachine.Chargers[_settler.Data.names.Race] = null;
            }

            _settler.Data.targets.TimeMachine = null;
            return _state = BTNodeState.Failure;
        }
    }
}