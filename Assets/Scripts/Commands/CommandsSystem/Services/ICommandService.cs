using System.Collections.Generic;

public interface ICommandService : IService {

	public Dictionary<int, CommandTarget> JobTargets { get;}
	public void HandleCommandRequest(CommandType type, bool withSelectedSettler);
	CommandTarget GetJob();
	void RegisterJob(int id, CommandTarget target);
	void UnregisterJob(int targetId);
}