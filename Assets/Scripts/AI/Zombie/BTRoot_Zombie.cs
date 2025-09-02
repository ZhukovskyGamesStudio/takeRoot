using AI.Node;

namespace AI {
	public class BTRoot_Zombie : Selector {
		public BTRoot_Zombie(Zombie zombie, ISettlersService settlers) {
			AddChild(new Behavior_Patrol(zombie));
			AddChild(new Behavior_FindNearbySettler(zombie, settlers));
		}
	}
}