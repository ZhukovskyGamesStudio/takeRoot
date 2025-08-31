using System.Collections.Generic;

namespace AI.Node {
	public class ParallelSelector : BTNode {
		private List<BTNode> _children = new(5);
		
		public override BTNodeState Evaluate() {
			foreach (BTNode child in _children) {
				BTNodeState result = child.Evaluate();

				if (result == BTNodeState.Success) {
					return _state = BTNodeState.Success;
				}
				
				if (result == BTNodeState.Running) {
					return _state = BTNodeState.Running;
				}
			}
			return _state = BTNodeState.Failure;
		}
		
		public ParallelSelector AddChild(BTNode child) {
			_children.Add(child);
			return this;
		}
	}
}