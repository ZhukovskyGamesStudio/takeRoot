using System;
using UniRx;
using UnityEngine;

public class CommandPresentation : IDisposable {

	public ReactiveProperty<CommandType> PendingCommand;
	
	private CommandView _view;
	
	private IJobCommandsInputHandlerService _jobCommandsInputHandler;

	public void Init(CommandView view, IJobCommandsInputHandlerService jobCommandsInputHandler) {
		_jobCommandsInputHandler = jobCommandsInputHandler;
		PendingCommand = _jobCommandsInputHandler.PendingCommand;

		_view = view;
		_view.SearchCommandButton.OnClickAsObservable().Subscribe(_ => PendingCommand.Value = CommandType.Search);
		_view.DestroyCommandButton.OnClickAsObservable().Subscribe(_ => PendingCommand.Value = CommandType.Destroy);
		
		PendingCommand.Subscribe(_ => _view.SearchCommandButton.interactable = PendingCommand.Value != CommandType.Search);
		PendingCommand.Subscribe(_ => _view.DestroyCommandButton.interactable = PendingCommand.Value != CommandType.Destroy);
	}

	public void Dispose() {
		PendingCommand?.Dispose();
	}
}