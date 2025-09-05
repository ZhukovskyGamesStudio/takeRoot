namespace AI.Node.Jobs {
	public class Job_Research : Sequence{
		public Job_Research(Settler settler, IResearchService researches) {
			AddChild(new Action_FindResearchStationWithResearch(settler, researches));
			var data = settler.Data;
			
			var move = new Selector()
				.AddChild(new ConditionalAction()
					.Do(new Action_MoveToPos(settler))
					.While(() => data.targets.ResearchStation && data.targets.ResearchStation.HasResearch))
				.AddChild(new Action_ClearResearch(settler));

			var search = new Selector()
				.AddChild(new ConditionalAction()
					.Do(new Action_Research(settler))
					.While(() => data.targets.ResearchStation && data.targets.ResearchStation.HasResearch))
				.AddChild(new Action_ClearResearch(settler));
			
			AddChild(move);
			AddChild(search);
			AddChild(new Action_ClearResearch(settler));
		}
	}
}