using System.Collections.Generic;
using AI.Node;

namespace AI {
	public class Selector : BTNode {
		private List<BTNode> _children = new List<BTNode>(5);


		public override BTNodeState Evaluate() {
			foreach (var child in _children) {
				var result = child.EvaluateWithDebug();

				if (result == BTNodeState.Success) {
					return BTNodeState.Success;
				}

				if (result == BTNodeState.Running) {
					return BTNodeState.Running;
				}
			}

			return BTNodeState.Failure;
		}
		
		public Selector AddChild(BTNode child) {
			_children.Add(child);
			return this;
		}
	}
}