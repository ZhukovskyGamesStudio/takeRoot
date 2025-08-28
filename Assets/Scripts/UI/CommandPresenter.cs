using System;
using UniRx;

public class CommandPresenter : IDisposable {
    public ReactiveProperty<JobType> PendingCommand;

    private CommandView _view;
    private IJobCommandsInputHandlerService _jobCommandsInputHandler;

    public CommandPresenter(CommandView view, IJobCommandsInputHandlerService jobCommandsInputHandler) {
        _jobCommandsInputHandler = jobCommandsInputHandler;
        PendingCommand = _jobCommandsInputHandler.PendingCommand;

        _view = view;

        foreach (var kvp in _view.Toggles) {
            kvp.Value.isOn = false;
            kvp.Value.onValueChanged.AddListener(isOn => {
                if (isOn) {
                    PendingCommand.Value = kvp.Key;
                } else {
                    if (PendingCommand.Value == kvp.Key) {
                        PendingCommand.Value = JobType.None;
                    }
                }
            });
        }
    }

    public void Dispose() {
        PendingCommand?.Dispose();
    }
}