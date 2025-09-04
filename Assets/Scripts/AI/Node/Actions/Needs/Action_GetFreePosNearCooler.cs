
namespace AI.Node.Jobs {
    public class Action_GetFreePosNearCooler : BTNode {
        private Settler _settler;

        public Action_GetFreePosNearCooler(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            _settler.Data.curMovePos = _settler.Data.targets.Cooler.InteractPos.position;
            return _state = BTNodeState.Success;
        }
    }
}