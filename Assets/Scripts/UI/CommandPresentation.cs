using System;
using UniRx;
using UnityEngine;

public class CommandPresentation : IDisposable {

	public ReactiveProperty<CommandType> PendingCommand;
	
	private CommandView _view;
	
	private ICommandInputHandlerService _commandInputHandler;

	public void Init(CommandView view, ICommandInputHandlerService commandInputHandler) {
		_commandInputHandler = commandInputHandler;
		PendingCommand = _commandInputHandler.PendingCommand;

		_view = view;
		_view.DebugCommandButton.OnClickAsObservable().Subscribe(_ => PendingCommand.Value = CommandType.Debug);
		_view.DestroyCommandButton.OnClickAsObservable().Subscribe(_ => PendingCommand.Value = CommandType.Destroy);
		
		PendingCommand.Subscribe(_ => _view.DebugCommandButton.interactable = PendingCommand.Value != CommandType.Debug);
		PendingCommand.Subscribe(_ => _view.DestroyCommandButton.interactable = PendingCommand.Value != CommandType.Destroy);
	}

	public void Dispose() {
		PendingCommand?.Dispose();
	}
}