namespace AI.Node {
	public abstract class BTNode {
		public string Name { get; }
		
		protected BTNodeState _state;
		public BTNodeState State => _state;
		protected BTNode(string name = null)
		{
			Name = name ?? GetType().Name;
		}
		public BTNodeState EvaluateWithDebug()
		{
			BTDebug.Enter(Name);
			var result = Evaluate();
			BTDebug.Log(result.ToString());
			BTDebug.Exit();
			return _state = result;
		}
	
		public abstract BTNodeState Evaluate();
	}

	public enum BTNodeState {
		Running,
		Success,
		Failure,
	}
}