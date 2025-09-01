using AI.Node;

namespace AI {
	public class Behavior_FindNearbySettler : Sequence {
		public Behavior_FindNearbySettler(Zombie zombie, ISettlersService settlers) {
			AddChild(new Action_FindNearbySettler(zombie, settlers));
		}
	}
}