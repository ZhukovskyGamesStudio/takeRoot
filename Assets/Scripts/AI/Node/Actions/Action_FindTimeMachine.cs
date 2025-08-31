using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_FindTimeMachine : BTNode {
        private Settler _settler;
        private ITimeScaleService _timeScaleService;

        public Action_FindTimeMachine(Settler settler, ITimeScaleService timeScaleService) {
            _settler = settler;
            _timeScaleService = timeScaleService;
        }

        public override BTNodeState Evaluate() {
            Race race = _settler.Data.names.Race;
            TimeMachine timeMachine = _timeScaleService.GetTimeMachine(race);
            
            if (timeMachine == null || timeMachine.Charged) {
                return _state = BTNodeState.Failure;
            }
            Vector3 movePos = timeMachine.InteractPos[race == Race.Plants ? 1 : 0].position;
            
            if (!_settler.Mover.HasPath(movePos)) {
                return _state = BTNodeState.Failure;
            }
            
            _settler.Data.targets.TimeMachine = timeMachine;
            timeMachine.Chargers[race] = _settler;
            _settler.Data.curMovePos = movePos;
            return _state = BTNodeState.Success;
        }
    }
}