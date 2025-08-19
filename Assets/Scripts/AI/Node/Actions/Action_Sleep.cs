namespace AI.Node.Jobs {
	public class Action_Sleep : BTNode {
		private Settler _settler;

		public Action_Sleep(Settler settler) {
			_settler = settler;
		}

		public override BTNodeState Evaluate() {
			if (!_settler.Data.isSleeping) {
				_settler.transform.position = _settler.Data.bed.SleepPos.position;
				_settler.Data.isSleeping = true;
				_settler.Data.energyChange = 5; //TODO: add bed multiplier
				_settler.Sleep();
			}
			if (_settler.Data.currentEnergy >= 100) {
				_settler.Data.energyChange = _settler.Data.defaultEnergyChange;
				_settler.transform.position = _settler.Data.bed.NearPos.position;
				_settler.WakeUp();
				return _state = BTNodeState.Success;
			}
			return _state = BTNodeState.Running;
		}
	}
}