using System.Collections.Generic;
using AI.Node;
using AI.Node.Jobs;

namespace AI {
	public class BTRoot : Selector {
		public BTRoot(Settler settler, ICommandService commands) : base(new List<BTNode>() {
			new Jobs(settler, commands)
		}) {
		}
	}
}