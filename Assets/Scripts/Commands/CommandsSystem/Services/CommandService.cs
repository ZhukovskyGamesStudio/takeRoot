using System.Collections.Generic;

public class CommandService : ICommandService {
	
	public Dictionary<int, CommandTarget> JobTargets { get; }= new Dictionary<int, CommandTarget>();
	
	public void HandleCommandRequest(CommandType type, bool withSelectedSettler) {
		switch (type) {
			case CommandType.Search:
				break;
			case CommandType.Destroy:
				break;
			case CommandType.Water:
				break;
		}
	}

	public CommandTarget GetJob() {
		foreach (var kvp in JobTargets) {
			var id = kvp.Key;
			var jobTarget = kvp.Value;
			
			if (jobTarget.Reserved == false)
				return jobTarget;
		}
		return null;
	}
	public void RegisterJob(int id, CommandTarget target, JobType jobType) {
		target.CurrentJobType = jobType;
		target.CurrentCommandId = id;
		JobTargets.Add(id, target);
	}
	public void UnregisterJob(CommandTarget target) {
		var id = target.CurrentCommandId;
		var jobTarget = JobTargets[id];
		jobTarget.CurrentCommandId = -1;
		jobTarget.CurrentJobType = JobType.None;
		JobTargets.Remove(id);
	}
}