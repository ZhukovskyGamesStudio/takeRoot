namespace AI.Node.Jobs {
    public class Action_ClearMovePosAfterIdleMove : BTNode {
        private readonly Settler _settler;

        public Action_ClearMovePosAfterIdleMove(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            _settler.Data.HasMovePos = false;
            _settler.Data.IdleMoveTimer = 0;
            _settler.Data.IsIdle = false;
            return _state = BTNodeState.Success;
        }
    }
}