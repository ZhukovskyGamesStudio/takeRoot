using System;

namespace AI.Node {
	public class DoUntil : BTNode {
		private readonly Action _action;
		private readonly Func<bool> _condition;

		public DoUntil(Action action, Func<bool> condition) {
			_action = action;
			_condition = condition;
		}
		
		
		public override BTNodeState Evaluate() {
			if (_condition()){
				return BTNodeState.Success;
			}
			_action?.Invoke();
			return BTNodeState.Running;
		}
	}
}