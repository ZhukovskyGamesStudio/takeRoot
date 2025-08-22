using System;
using UniRx;

public class SelectionServicePresenter : IDisposable {
	private readonly ISelectionService _selection;

	public ReactiveProperty<bool> SelectionEnabled;
	private InfoBookView _infoBookView;
	
	
	public SelectionServicePresenter(InfoBookView infoBookView, IJobCommandsInputHandlerService commandInputHandler, ISelectionService selection) {
		_infoBookView = infoBookView;
		_selection = selection;
		SelectionEnabled = _selection.IsEnabled;
		commandInputHandler.PendingCommand.Subscribe(_ => SelectionEnabled.Value = commandInputHandler.PendingCommand.Value == JobType.None);
		selection.SelectedReactive.Subscribe(SwitchPanel);
	}

	private void SwitchPanel(Selectable selectable) {
		_infoBookView.gameObject.SetActive(selectable!= null);
		_infoBookView.SetData(selectable);
	}

	public void Dispose() {
		SelectionEnabled?.Dispose();
	}
}