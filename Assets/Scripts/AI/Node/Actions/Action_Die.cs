namespace AI.Node.Jobs {
    public class Action_Die : BTNode {
        private Settler _settler;
        private DeathCause _deathCause;

        public Action_Die(Settler settler, DeathCause cause) {
            _settler = settler;
            _deathCause = cause;
        }

        public override BTNodeState Evaluate() {
            _settler.Die(_deathCause);
            
            return _state = BTNodeState.Success;
        }
    }
}