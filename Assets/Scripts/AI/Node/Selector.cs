using System.Collections.Generic;
using AI.Node;

namespace AI.Node {
	public class Selector : BTNode {
		private List<BTNode> _children = new List<BTNode>(5);
		private int _currentChild = 0;

		public override BTNodeState Evaluate() {
			while (_currentChild < _children.Count) {
				var result = _children[_currentChild].EvaluateWithDebug();

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