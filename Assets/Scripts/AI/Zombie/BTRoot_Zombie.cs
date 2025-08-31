using AI.Node;

namespace AI {
	public class BTRoot_Zombie : Sequence{
		public BTRoot_Zombie(Zombie zombie) {
			AddChild(new Behavior_Patrol(zombie));
		}
	}
}