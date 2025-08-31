using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_ChargeTimeMachine : BTNode {
        private Settler _settler;

        public Action_ChargeTimeMachine(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            TimeMachine timeMachine = _settler.Data.targets.TimeMachine;

            if (timeMachine.Charged) {
                _settler.TimeMachineCharger.Cancel();
                timeMachine.Chargers[_settler.Data.names.Race] = null;
                _settler.Data.targets.TimeMachine = null;
                
                return _state = BTNodeState.Success;
            }
            
            _settler.TimeMachineCharger.Charge(timeMachine, _settler.Data.names.Race);
            return _state = BTNodeState.Running;
        }
    }
}