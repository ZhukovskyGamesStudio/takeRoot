using System.Collections.Generic;

namespace AI.Node.Jobs {
	public class Job_Destroy: JobRunner {
		private Settler _settler;

		public Job_Destroy(Settler settler) : base(settler) {
			JobType = JobType.Destroy;
			SetOnFailed(() => !settler.Data.currTarget);
			AddChild(new Conditional(() => settler.Data.currJob == JobType.Destroy));
			AddChild(new Action_MoveTo(settler));
			AddChild(new Action_Hit(settler));
		}

		protected override void OnFailedAction() {
			_settler.Destroyer.Cancel();
		}
	}
}