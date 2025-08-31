using AI.Node;

namespace AI {
	public class Behavior_Patrol : Sequence {
		public Behavior_Patrol(Zombie zombie) {
			var move = new ConditionalAction()
				.Do(new Action_ZombieMove(zombie))
				.While(() => !zombie.Data.Target);

			AddChild(new Conditional(() => !zombie.Data.Target));
			AddChild(new Action_PickPatrolPos(zombie));
			//AddChild(move);
		}
	}
}