using System.Data;

public interface ICommandInputHandlerService : IService {
	public CommandType PendingCommand { get; set; }
	public bool IsEnabled { get; set; }
	
}