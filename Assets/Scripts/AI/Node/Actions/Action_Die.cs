namespace AI.Node.Jobs {
    public class Action_Die : BTNode {
        private Settler _settler;

        public Action_Die(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            _settler.Die();
            
            return _state = BTNodeState.Success;
        }
    }
}