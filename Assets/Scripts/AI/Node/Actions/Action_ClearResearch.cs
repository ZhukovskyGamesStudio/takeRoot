namespace AI.Node.Jobs {
	public class Action_ClearResearch : BTNode {
		private readonly Settler _settler;

		public Action_ClearResearch(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			var data = _settler.Data;
			var researchStation = data.targets.ResearchStation;
			if (researchStation) {
				if (data.names.Race == Race.Plants) {
					researchStation.plantResearcher = null;
				} else if (data.names.Race == Race.Robots) {
					researchStation.robotResearcher = null;
				}
			}
			data.targets.ResearchStation = null;
			return BTNodeState.Success;
		}
	}
}