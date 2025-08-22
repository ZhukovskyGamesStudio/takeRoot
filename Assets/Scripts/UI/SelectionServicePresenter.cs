using System;
using UniRx;

public class SelectionServicePresenter : IDisposable {
    private readonly ISelectionService _selection;

    public ReactiveProperty<bool> SelectionEnabled;
    private InfoBookView _infoBookView;
    private SettlerPanel _settlerPanel;

    public SelectionServicePresenter(InfoBookView infoBookView, SettlerPanel settlerPanel, IJobCommandsInputHandlerService commandInputHandler,
        ISelectionService selection) {
        _infoBookView = infoBookView;
        _settlerPanel = settlerPanel;
        _selection = selection;
        SelectionEnabled = _selection.IsEnabled;
        commandInputHandler.PendingCommand.Subscribe(_ => SelectionEnabled.Value = commandInputHandler.PendingCommand.Value == JobType.None);
        selection.SelectedReactive.Subscribe(SwitchPanel);
    }

    private void SwitchPanel(Selectable selectable) {
        if (selectable is SettlerSelectable) {
            _settlerPanel.gameObject.SetActive(true);
            _settlerPanel.SetData((AI.SettlerData)selectable.GetData());
            _infoBookView.gameObject.SetActive(false);
        } else if (selectable is CommandTargetSelectable) {
            _settlerPanel.gameObject.SetActive(false);
            _infoBookView.gameObject.SetActive(selectable != null);
            _infoBookView.SetData((CommandTargetData)selectable.GetData());
        } else {
            _settlerPanel.gameObject.SetActive(false);
            _infoBookView.gameObject.SetActive(false);
        }
    }

    public void Dispose() {
        SelectionEnabled?.Dispose();
    }
}