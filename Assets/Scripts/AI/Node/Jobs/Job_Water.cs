namespace AI.Node.Jobs {
	public class Job_Water : Sequence {
		
		public Job_Water(Settler settler) {
			AddChild(new Conditional(() => settler.Data.currTarget));
			AddChild(new Conditional(() => settler.Data.currTarget.CurrentJobType == JobType.Water));
			AddChild(new Action_MoveTo(settler));
			AddChild(new Action_Water(settler));
		}
	}
}