using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CommandPresenter : IDisposable {

	public ReactiveProperty<JobType> PendingCommand;
	
	private CommandView _view;
	private IJobCommandsInputHandlerService _jobCommandsInputHandler;

	public void Init(CommandView view, IJobCommandsInputHandlerService jobCommandsInputHandler) {
		_jobCommandsInputHandler = jobCommandsInputHandler;
		PendingCommand = _jobCommandsInputHandler.PendingCommand;

		_view = view;
		SubscribeCommand(JobType.Search, _view.SearchCommandButton);
		SubscribeCommand(JobType.Destroy, _view.DestroyCommandButton);
		SubscribeCommand(JobType.Water, _view.WaterCommandButton);
		SubscribeCommand(JobType.Cancel, _view.CancelCommandButton);
		
		PendingCommand.Subscribe(_ => _view.CurrentCommand.text = PendingCommand.Value.ToString());
		
	}

	private void SubscribeCommand(JobType type, Button button) {
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