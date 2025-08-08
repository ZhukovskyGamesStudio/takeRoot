using System.Collections.Generic;

namespace AI.Node.Jobs {
	public class Job_Destroy: Sequence {
		private Settler _settler;
		
		public Job_Destroy(Settler settler) : base(new List<BTNode>() {
			//new Action_MoveTo(settler.Mover, settler.Data.currTarget),
		}) { }
	}
}