using System.Collections.Generic;

public class CommandService : ICommandService {
	
	public Dictionary<int, BaseCommand> Commands { get; }= new Dictionary<int, BaseCommand>();
	
	
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
	public void CancelCommand(int id) {
		var command = Commands[id];
		command.Cancel();
	}
	public void RegisterCommand(int id, BaseCommand baseCommand) {
		Commands.Add(id, baseCommand);
	}
	public void UnregisterCommand(int id) {
		Commands.Remove(id);
	}
}