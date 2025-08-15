using System.Collections.Generic;

namespace AI.Node.Jobs {
	public class Job_Destroy: Sequence {
		private Settler _settler;

		public Job_Destroy(Settler settler) {
			AddChild(new Conditional(() => settler.Data.currTarget));
			AddChild(new Conditional(() => settler.Data.currTarget.CurrentJobType == JobType.Destroy));
			AddChild(new Action_MoveTo(settler));
			AddChild(new Action_Hit(settler));
		}

	}
}