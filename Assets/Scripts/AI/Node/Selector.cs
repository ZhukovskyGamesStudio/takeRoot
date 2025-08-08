using System.Collections.Generic;
using AI.Node;

namespace AI {
	public class Selector : BTNode {
		private List<BTNode> _children;

		public Selector(List<BTNode> children) {
			_children = children;
		}

		public override BTNodeState Evaluate() {
			foreach (var child in _children) {
				var result = child.Evaluate();

				if (result == BTNodeState.Success) {
					return BTNodeState.Success;
				}

				if (result == BTNodeState.Running) {
					return BTNodeState.Running;
				}
			}

			return BTNodeState.Failure;
		}
	}
}