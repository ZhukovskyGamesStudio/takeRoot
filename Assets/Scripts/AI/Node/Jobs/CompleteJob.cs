namespace AI.Node.Jobs {
	public class CompleteJob : BTNode {
		private Settler _settler;
		
		public CompleteJob(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			_settler.Data.currJob = JobType.None;
			_settler.Data.currTarget = null;
			return BTNodeState.Success;
		}
	}
}