using System;
using UniRx;
using UnityEngine.UI;

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
        PendingCommand.AsObservable().Subscribe(_ => UpdateToggles());
    }

    private void UpdateToggles() {
        Toggle enabledToggle = null;
        foreach (var kvp in _view.Toggles) {
            var jobType = kvp.Key;
            var toggle = kvp.Value;
            if (toggle.isOn)
                enabledToggle = toggle;
            if (PendingCommand.Value == jobType) {
                toggle.isOn = true;
                break;
            }
        }
        if (enabledToggle != null) {
            enabledToggle.isOn = false;
        }
    }
    public void Dispose() {
        PendingCommand?.Dispose();
    }
}