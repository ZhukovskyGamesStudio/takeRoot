using System.Collections.Generic;

namespace AI.Node {
    public class Selector : BTNode {
        private List<BTNode> _children = new(5);
        private int _currentChild = 0;

        public override BTNodeState Evaluate() {
            while (_currentChild < _children.Count) {
                BTNodeState result = _children[_currentChild].EvaluateWithDebug();

                if (result == BTNodeState.Success) {
                    _currentChild = 0;
                    return _state = BTNodeState.Success;
                }

                if (result == BTNodeState.Running) {
                    return _state = BTNodeState.Running;
                }

                _currentChild++;
            }

            _currentChild = 0;
            return _state = BTNodeState.Failure;
        }

        public Selector AddChild(BTNode child) {
            _children.Add(child);
            return this;
        }
    }
}