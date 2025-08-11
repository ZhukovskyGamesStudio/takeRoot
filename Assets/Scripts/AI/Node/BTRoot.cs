using System.Collections.Generic;
using AI.Behaviors;
using AI.Node;
using AI.Node.Jobs;

namespace AI {
	public class BTRoot : Selector {
		public BTRoot(Settler settler, ICommandService commands) {
			AddChild(new Jobs(settler, commands));
			AddChild(new Behavior_Idle(settler));
		}
	}
}