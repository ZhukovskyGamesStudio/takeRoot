namespace AI.Node.Jobs {
	public class ResetJobOnSettler : BTNode{
		private Settler _settler;
		
		public ResetJobOnSettler(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			_settler.Data.currJob = JobType.None;
			_settler.Data.currTarget = null;
			return BTNodeState.Failure;
		}
	}
}