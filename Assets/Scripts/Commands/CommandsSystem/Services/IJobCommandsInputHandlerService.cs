using System.Data;
using UniRx;

public interface IJobCommandsInputHandlerService : IService {
	public ReactiveProperty<JobType> PendingCommand { get; set; }
	public bool IsEnabled { get; set; }
	
}