using System;

namespace AI.Node {
    public class Conditional : BTNode {
        private Func<bool> _condition;

        public Conditional(Func<bool> condition) {
            _condition = condition;
        }

        public override BTNodeState Evaluate() {
            _state = _condition() ? BTNodeState.Success : BTNodeState.Failure;
            return _state;
        }
    }
}