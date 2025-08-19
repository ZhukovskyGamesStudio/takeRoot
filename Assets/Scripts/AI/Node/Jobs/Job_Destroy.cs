using System;
using System.Collections.Generic;

namespace AI.Node.Jobs {
	public class Job_Destroy: Sequence {
		private Settler _settler;

		public Job_Destroy(Settler settler) {
			var data = settler.Data;
			Func<bool> condition = () =>  data.currTarget &&
			                    data.currTarget.Data.CurrentJob == JobType.Destroy &&
			                    data.currJob == JobType.Destroy;
			var move = new ConditionalAction()
				.Do(new Action_MoveTo(settler))
				.While(condition);
			var hit = new ConditionalAction()
				.Do(new Action_Hit(settler))
				.While(condition);
			
			AddChild(move);
			AddChild(hit);
		}

	}
}