namespace AI.Node {
    public class Inverter : BTNode {
        private BTNode _child;

        public Inverter(BTNode child) {
            _child = child;
        }

        public override BTNodeState Evaluate() {
            BTNodeState childState = _child.Evaluate();

            switch (childState) {
                case BTNodeState.Success:
                    return BTNodeState.Failure;
                case BTNodeState.Failure:
                    return BTNodeState.Success;
                case BTNodeState.Running:
                    return BTNodeState.Running;
                default:
                    return BTNodeState.Failure;
            }
        }
    }
}