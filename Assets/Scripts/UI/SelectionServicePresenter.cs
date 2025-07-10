using System;
using UniRx;

public class SelectionServicePresenter : IDisposable {
	private readonly ISelectionService _selection;

	public ReactiveProperty<bool> SelectionEnabled;
	
	
	public SelectionServicePresenter(IJobCommandsInputHandlerService commandInputHandler, ISelectionService selection) {
		_selection = selection;
		SelectionEnabled = _selection.IsEnabled;
		commandInputHandler.PendingCommand.Subscribe(_ => SelectionEnabled.Value = commandInputHandler.PendingCommand.Value == CommandType.None);
	}

	public void Dispose() {
		SelectionEnabled?.Dispose();
	}
}