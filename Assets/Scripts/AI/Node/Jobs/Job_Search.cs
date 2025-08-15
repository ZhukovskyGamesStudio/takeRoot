using System.Collections.Generic;
using AI.Node.Conditions;

namespace AI.Node.Jobs {
	public class Job_Search : Sequence {
		public Job_Search(Settler settler) { 
			AddChild(new Conditional(() => settler.Data.currTarget));
			AddChild(new Conditional(() => settler.Data.currTarget.CurrentJobType == JobType.Search));
			AddChild(new Action_MoveTo(settler));
			AddChild(new Action_SearchObject(settler));
		}
	}
}