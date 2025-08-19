namespace AI.Node.Jobs {
	public class Action_SleepOnGround : BTNode {
		private Settler _settler;

		public Action_SleepOnGround(Settler settler) {
			_settler = settler;
		}

		public override BTNodeState Evaluate() {
			if (!_settler.Data.isSleeping) {
				_settler.Sleep();
				_settler.Data.energyChange = _settler.Data.onGroundEnergyChange;
			}
			if (!_settler.Data.IsTired) {
				_settler.Data.energyChange = _settler.Data.defaultEnergyChange;
				_settler.WakeUp();
				return _state = BTNodeState.Success;
			}
			return _state = BTNodeState.Running;
		}
	}
}