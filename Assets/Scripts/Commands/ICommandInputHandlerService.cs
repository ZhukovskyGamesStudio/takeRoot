using System.Data;
using UniRx;

public interface ICommandInputHandlerService : IService {
	public ReactiveProperty<CommandType> PendingCommand { get; set; }
	public bool IsEnabled { get; set; }
	
}