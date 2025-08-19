using System;

namespace AI.Node {
	public class ConditionalAction : BTNode {
		private Func<bool> _condition;
		private BTNode _child;
		public override BTNodeState Evaluate() {
			if (_condition()) {
				return _state = _child.Evaluate();
			}
			return _state = BTNodeState.Failure;
		}

		public ConditionalAction Do(BTNode action) {
			_child = action;
			return this;
		}
		public ConditionalAction While(Func<bool> condition) {
			_condition = condition;
			return this;
		}
	}
}