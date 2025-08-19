using System.Collections.Generic;

namespace AI.Node {
	public class Sequence : BTNode {
		private List<BTNode> _children = new List<BTNode>(10);
		private int _currentChild = 0;
		
		public override BTNodeState Evaluate() {
			while (_currentChild < _children.Count) {
				var result = _children[_currentChild].EvaluateWithDebug();

				if (result == BTNodeState.Failure) {
					_currentChild = 0;
					return _state = BTNodeState.Failure;
				}

				if (result == BTNodeState.Running) {
					return _state = BTNodeState.Running;
				}
				
				_currentChild++;
			}
			_currentChild = 0;
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