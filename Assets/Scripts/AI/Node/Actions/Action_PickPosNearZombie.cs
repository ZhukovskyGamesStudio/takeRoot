namespace AI.Node.Jobs {
	public class Action_PickPosNearZombie : BTNode {
		private readonly Settler _settler;

		public Action_PickPosNearZombie(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			_settler.Data.tactical.TacticalMovePos = _settler.Data.tactical.Target.transform.position;
			_settler.Data.tactical.HasTacticalMovePos = true;
			return BTNodeState.Success;
		}
	}
}