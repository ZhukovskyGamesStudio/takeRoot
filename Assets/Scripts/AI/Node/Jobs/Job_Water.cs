using System;

namespace AI.Node.Jobs {
	public class Job_Water : Sequence {
		private Settler _settler;
		
		public Job_Water(Settler settler) {
			var data = settler.Data;
			Func<bool> condition = () =>  data.currTarget &&
			                              data.currTarget.Data.CurrentJob == JobType.Water &&
			                              data.currJob == JobType.Water &&
			                              !data.IsTactical;
			var move = new ConditionalAction()
				.Do(new Action_MoveTo(settler))
				.While(condition);
			var water = new ConditionalAction()
				.Do(new Action_Water(settler))
				.While(condition);
			
			AddChild(move);
			AddChild(water);
		}
	}
}