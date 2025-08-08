using System.Collections.Generic;

namespace AI.Node.Jobs {
	public class Job_Search : Sequence {
		private Settler _settler;
		
		public Job_Search(Settler settler) : base(new List<BTNode>() {
			new Conditional(() => settler.Data.currJob == JobType.Search),
			new Action_MoveTo(settler),
			new Action_SearchObject(settler)
		}) {}
	}
}