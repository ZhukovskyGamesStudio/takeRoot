using System.Collections.Generic;

namespace AI.Node.Jobs {
	public class Job_Destroy: Sequence {
		private Settler _settler;
		
		public Job_Destroy(Settler settler) : base(new List<BTNode>() {
			new Conditional(() => settler.Data.currJob == JobType.Destroy),
			new Action_MoveTo(settler),
			new Action_Hit(settler)
		}) { }
	}
}