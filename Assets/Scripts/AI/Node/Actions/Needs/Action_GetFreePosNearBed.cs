namespace AI.Node.Jobs {
    public class Action_GetFreePosNearBed : BTNode {
        private Settler _settler;

        public Action_GetFreePosNearBed(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            _settler.Data.curMovePos = _settler.Data.targets.Bed.NearPos.position;
            return _state = BTNodeState.Success;
        }
    }
}