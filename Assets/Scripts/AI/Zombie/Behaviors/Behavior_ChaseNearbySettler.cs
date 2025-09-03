using AI.Node;
using AI.Node.Jobs;

namespace AI {
	public class Behavior_ChaseNearbySettler : Sequence {
		public Behavior_ChaseNearbySettler(Zombie zombie, ISettlersService settlers) {
			AddChild(new Action_FindNearbySettler(zombie, settlers));
			AddChild(new Action_PickPositionNearSettler(zombie));
			AddChild(new ConditionalAction()
				.Do(new Action_ZombieMove(zombie))
				.While(() => zombie.Data.CurrMovePos == zombie.Data.Target.transform.position));
		}
	}
}