using System;
using UniRx;
[Obsolete]
public class CommandPresentation : IDisposable {
    public ReactiveProperty<JobType> PendingCommand;

    private CommandView _view;

    private IJobCommandsInputHandlerService _jobCommandsInputHandler;

    public void Init(CommandView view, IJobCommandsInputHandlerService jobCommandsInputHandler) {
        _jobCommandsInputHandler = jobCommandsInputHandler;
        PendingCommand = _jobCommandsInputHandler.PendingCommand;

        _view = view;

        foreach (var kvp in _view.Toggles) {
            kvp.Value.isOn = false;
            kvp.Value.onValueChanged.AddListener(isOn => {
                if (isOn) {
                    PendingCommand.Value = kvp.Key;
                }
            });
        }

        //_view.SearchCommandButton.OnClickAsObservable().Subscribe(_ => PendingCommand.Value = JobType.Search);
        //_view.DestroyCommandButton.OnClickAsObservable().Subscribe(_ => PendingCommand.Value = JobType.Destroy);

        //PendingCommand.Subscribe(_ => _view.SearchCommandButton.interactable = PendingCommand.Value != JobType.Search);
        //PendingCommand.Subscribe(_ => _view.DestroyCommandButton.interactable = PendingCommand.Value != JobType.Destroy);
    }

    public void Dispose() {
        PendingCommand?.Dispose();
    }
}