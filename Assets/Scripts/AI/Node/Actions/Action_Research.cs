namespace AI.Node.Jobs {
	public class Action_Research : BTNode {
		private readonly Settler _settler;

		public Action_Research(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			var data = _settler.Data;
			var researchStation = data.targets.ResearchStation;
			if (data.names.Race == Race.Plants) {
				if (!researchStation.IsAnotherOnPosition(Race.Robots))
					return BTNodeState.Running;
			} else if (data.names.Race == Race.Robots) {
				if (!researchStation.IsAnotherOnPosition(Race.Plants))
					return BTNodeState.Running;
			}
			_settler.Researcher.Research(researchStation);
			return BTNodeState.Running;
		}
	}
}