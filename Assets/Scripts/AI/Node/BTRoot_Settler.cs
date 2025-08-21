using System.Collections.Generic;
using AI.Behaviors;
using AI.Node;
using AI.Node.Jobs;

namespace AI {
	public class BTRoot_Settler : Selector {
		public BTRoot_Settler(Settler settler, ICommandService commands) {
			AddChild(new Behavior_CriticalTired(settler));
			AddChild(new Behavior_Tactical(settler));
			AddChild(new Behavior_Energy(settler));
			AddChild(new Jobs(settler, commands));
			//AddChild(new Behavior_Idle(settler));
		}
	}
}