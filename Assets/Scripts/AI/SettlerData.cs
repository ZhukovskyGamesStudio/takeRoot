namespace AI {
	public class SettlerData {
		public bool IsTactical;
		public bool CanDoJob() => true; //TODO: make condition
		
		public JobType currJob;
		public CommandTarget currTarget;
		
		public bool HasJob => currJob != JobType.None;

	}
}