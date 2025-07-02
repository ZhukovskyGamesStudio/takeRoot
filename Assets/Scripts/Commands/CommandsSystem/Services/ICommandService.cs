using System.Collections.Generic;

public interface ICommandService : IService {

	public Dictionary<int, BaseCommand> Commands { get;}
	public void HandleCommandRequest(CommandType type, bool withSelectedSettler);
	void CancelCommand(int id);
	void RegisterCommand(int id, BaseCommand baseCommand);
	void UnregisterCommand(int id);
}