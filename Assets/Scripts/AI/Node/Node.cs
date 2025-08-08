namespace AI.Node {
	public abstract class BTNode {
		protected BTNodeState _state;
	
		public BTNodeState State => _state;
	
		public abstract BTNodeState Evaluate();
	}

	public enum BTNodeState {
		Running,
		Success,
		Failure,
	}
}