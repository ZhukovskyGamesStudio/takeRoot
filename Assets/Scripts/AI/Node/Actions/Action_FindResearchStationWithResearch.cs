namespace AI.Node.Jobs {
	public class Action_FindResearchStationWithResearch : BTNode{
		private readonly Settler _settler;
		private readonly IResearchService _researches;

		public Action_FindResearchStationWithResearch(Settler settler, IResearchService researches) {
			_settler = settler;
			_researches = researches;
		}
		public override BTNodeState Evaluate() {
			var data = _settler.Data;
			ResearchStation researchStation = _researches.GetResearchStationWithResearch(data.names.Race);
			if (researchStation == null) {
				return BTNodeState.Failure;
			}

			if (data.names.Race == Race.Plants) {
				researchStation.plantResearcher = _settler;
				data.curMovePos = researchStation.plantInteractPosition.position;
			}
			else if (data.names.Race == Race.Robots){
				researchStation.robotResearcher = _settler;
				data.curMovePos = researchStation.robotInteractPosition.position;
			}
			data.targets.ResearchStation = researchStation;
			return BTNodeState.Success;
		}
	}
}