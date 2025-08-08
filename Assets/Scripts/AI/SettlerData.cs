namespace AI {
	public class SettlerData {
		public bool IsTactical;
		public bool CanDoJob() => true; //TODO: make condition
		
		public JobType currJob;
		public CommandTarget currTarget;

		public float HitTime = 1.3f;
		
		public bool HasJob => currJob != JobType.None;
		

	}
}