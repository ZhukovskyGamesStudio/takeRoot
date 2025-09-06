using System;
using UniRx;

public interface IJobCommandsInputHandlerService : IService {
    public Action<JobType> OnJobChanged { get; set; }
    public ReactiveProperty<JobType> PendingCommand { get; set; }
    public bool IsEnabled { get; set; }
}