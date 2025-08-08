using System.Collections.Generic;

namespace AI.Node.Jobs {
	public class Jobs : Sequence {
		public Jobs(Settler settler) : base(new List<BTNode>() {
			new Selector(new List<BTNode>() {
				new Conditional(() => settler.Data.HasJob),
				new Action_FindJob(settler)
			}),
			new Conditional(() => settler.Data.HasJob),
			new Selector(new List<BTNode>(){		
				new Job_Search(settler),
				new Job_Destroy(settler),
				new ResetJobOnSettler(settler)
			}),
			new CompleteJob(settler)
		}) { }
	}
}

public enum JobType {
	None,
	Search,
	Destroy
}