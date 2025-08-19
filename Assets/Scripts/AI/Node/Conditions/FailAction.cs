using System;

namespace AI.Node.Conditions {
	public class FailConditionAction : BTNode {
		private readonly Conditional _condition;
		private readonly Action _action;

		public FailConditionAction(Conditional condition, Action action) {
			_condition = condition;
			_action = action;
		}
		public override BTNodeState Evaluate() {
			var state = _condition.Evaluate();
			if (state == BTNodeState.Success) {
				_action?.Invoke();
				return BTNodeState.Failure;
			}
			if (state == BTNodeState.Failure) {
				return BTNodeState.Success;
			}
			return BTNodeState.Running;
		}
	}
}