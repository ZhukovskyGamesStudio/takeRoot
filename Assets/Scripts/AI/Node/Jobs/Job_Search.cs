using System.Collections.Generic;
using AI.Node.Conditions;

namespace AI.Node.Jobs {
	public class Job_Search : JobRunner {
		public Job_Search(Settler settler) : base(settler) { 
			JobType = JobType.Search;
			SetOnFailed(() => !settler.Data.currTarget);
			AddChild(new Action_MoveTo(settler));
			AddChild(new Action_SearchObject(settler));
		}

		protected override void OnFailedAction() {
			Settler.Searcher.Cancel();
		}
	}
}