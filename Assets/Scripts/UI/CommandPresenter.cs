using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CommandPresenter : IDisposable {

	public ReactiveProperty<CommandType> PendingCommand;
	
	private CommandView _view;
	private IJobCommandsInputHandlerService _jobCommandsInputHandler;

	public void Init(CommandView view, IJobCommandsInputHandlerService jobCommandsInputHandler) {
		_jobCommandsInputHandler = jobCommandsInputHandler;
		PendingCommand = _jobCommandsInputHandler.PendingCommand;

		_view = view;
		SubscribeCommand(CommandType.Search, _view.SearchCommandButton);
		SubscribeCommand(CommandType.Destroy, _view.DestroyCommandButton);
		SubscribeCommand(CommandType.Water, _view.WaterCommandButton);
		SubscribeCommand(CommandType.Cancel, _view.CancelCommandButton);
		
		PendingCommand.Subscribe(_ => _view.CurrentCommand.text = PendingCommand.Value.ToString());
		
	}

	private void SubscribeCommand(CommandType type, Button button) {
		button.OnClickAsObservable().Subscribe(_ => PendingCommand.Value = type);
		PendingCommand.Subscribe(_ =>
		{
			button.image.sprite = PendingCommand.Value == type ? _view.offButton : _view.onButton; 
		});
	}
	
	public void Dispose() {
		PendingCommand?.Dispose();
	}
}