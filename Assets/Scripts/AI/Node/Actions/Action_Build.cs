namespace AI.Node.Jobs {
	public class Action_Build : BTNode {
		private readonly Settler _settler;

		public Action_Build(Settler settler) {
			_settler = settler;
		}

		public override BTNodeState Evaluate() {
			var blueprint = _settler.Data.building.buildingBlueprint;
			if (blueprint.WasBuilded) {
				blueprint.Build();
				_settler.Data.building.buildingBlueprint = null;
				return _state = BTNodeState.Success;
			}
			_settler.Builder.Build(_settler.Data.building.buildingBlueprint);
			return _state = BTNodeState.Running;
		}
	}
}