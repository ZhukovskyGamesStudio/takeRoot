using System.Collections.Generic;

namespace AI.Node {
	public class Sequence : BTNode {
		private List<BTNode> _children = new List<BTNode>(10);
		
		public override BTNodeState Evaluate() {
			foreach (BTNode child in _children) {
				var result = child.EvaluateWithDebug();

				if (result == BTNodeState.Failure) {
					return _state = BTNodeState.Failure;
				}

				if (result == BTNodeState.Running) {
					return _state = BTNodeState.Running;
				}
			}
			return _state = BTNodeState.Success;
		}

		public Sequence AddChild(BTNode child) {
			_children.Add(child);
			return this;
		}

		protected void InsertChild(int index, BTNode child) {
			_children.Insert(index, child);
		}
	}
}