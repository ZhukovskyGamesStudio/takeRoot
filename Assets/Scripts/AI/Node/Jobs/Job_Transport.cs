using System.Collections.Generic;

namespace AI.Node.Jobs {
/*
	public class Job_Transport : Sequence {
		public Job_Transport(Settler settler) : base(new List<BTNode>() {
			new Conditional(() => settler.Data.currJob == JobType.Transport),
			new Selector(new List<BTNode>() {
				new Conditional(() => settler.Data.subsequentTarget != null),
				new Action_FindStorage(settler)
			}),
			new Conditional(() => settler.Data.subsequentTarget != null),
			new Sequence(new List<BTNode>() {
				new Conditional(() => !settler.Data.HasItem),
				new Action_MoveTo(settler),
				new Action_Pickup(settler)
			}),
			new Sequence(new List<BTNode>() {
				new Conditional(() => settler.Data.HasItem),
				new Action_MoveTo(settler, true),
				new Action_Store(settler)
			})
		}) { }
	}
*/
}