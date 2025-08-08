using System.Collections.Generic;

namespace AI.Node {
	public class Sequence : BTNode {
		private List<BTNode> _children;

		public Sequence(List<BTNode> children) {
			_children = children;
		}
		
		public override BTNodeState Evaluate() {
			foreach (BTNode child in _children) {
				var result = child.Evaluate();

				if (result == BTNodeState.Failure) {
					return _state = BTNodeState.Failure;
				}

				if (result == BTNodeState.Running) {
					return _state = BTNodeState.Running;
				}
			}
			return _state = BTNodeState.Success;
		}
	}
}