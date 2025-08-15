using System;
using System.Collections.Generic;

namespace AI.Node.Jobs {
	public class Jobs : Sequence {
		public Jobs(Settler settler, ICommandService commands) {
			var findJob = new Selector()
				.AddChild(new Conditional(() =>
					settler.Data.HasJob))
				.AddChild(new Action_FindJob(settler, commands))
				.AddChild(new ResetJobOnSettler(settler));
			AddChild(findJob);
			var jobsBehavior = new Selector() //TODO: add priority selector
				.AddChild(new Job_Search(settler))
				.AddChild(new Job_Destroy(settler))
				.AddChild(new Job_Water(settler))
				.AddChild(new ResetJobOnSettler(settler));
			AddChild(jobsBehavior);
		}
	}
}

[Flags][Serializable]
public enum JobType {
	None = 0,
	Search = 1 << 1,
	Destroy = 1 << 2,
	Water = 1 << 3,
	Transport = 1 << 4,
	Cancel = 1 << 5,
}
