using System;
using UniRx;

public class SelectionServicePresentation : IDisposable {
	private readonly ISelectionService _selection;

	public ReactiveProperty<bool> SelectionEnabled;
	
	
	public SelectionServicePresentation(CommandPresentation commandPresentation, ISelectionService selection) {
		_selection = selection;
		SelectionEnabled = new ReactiveProperty<bool>(_selection.IsEnabled);
		SelectionEnabled.AsObservable().Subscribe(_ => _selection.IsEnabled = SelectionEnabled.Value);
		
		commandPresentation.PendingCommand.Subscribe(_ => SelectionEnabled.Value = commandPresentation.PendingCommand.Value == CommandType.None);
	}

	public void Dispose() {
		SelectionEnabled?.Dispose();
	}
}