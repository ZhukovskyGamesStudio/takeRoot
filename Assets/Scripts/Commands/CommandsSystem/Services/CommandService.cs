using System.Collections.Generic;

public class CommandService : ICommandService {
	
	public Dictionary<int, BaseCommand> Commands = new Dictionary<int, BaseCommand>();
	
	
	public void HandleCommandRequest(CommandType type, bool withSelectedSettler) {
		switch (type) {
			case CommandType.Cancel:
				//RayCast + cancel command via CommandTarge.CurrentCommandId = -1;
				break;
			case CommandType.Search:
				break;
			case CommandType.Destroy:
				break;
			case CommandType.Move:
				break;
			
		}
	}

	public void CancelCommand(int id) {
		var command = Commands[id];
		command.Cancel();
		Commands.Remove(id);
	}
	public void RegisterCommand(int id, BaseCommand baseCommand) {
		Commands.Add(id, baseCommand);
	}
	public void UnregisterCommand(int id) {
		Commands.Remove(id);
	}
}