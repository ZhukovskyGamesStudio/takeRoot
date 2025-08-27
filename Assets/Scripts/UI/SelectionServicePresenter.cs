using System;
using UniRx;

public class SelectionServicePresenter : IDisposable {
    private readonly ISelectionService _selection;

    public ReactiveProperty<bool> SelectionEnabled;
    private InfoPanelView _infoPanelView;
    private SettlerInfoPanel _settlerPanel;

    public SelectionServicePresenter(InfoPanelView infoPanelView, SettlerInfoPanel settlerPanel,
        IJobCommandsInputHandlerService commandInputHandler, ISelectionService selection) {
        _infoPanelView = infoPanelView;
        _settlerPanel = settlerPanel;
        _selection = selection;
        SelectionEnabled = _selection.IsEnabled;
        commandInputHandler.PendingCommand.Subscribe(_ => SelectionEnabled.Value = commandInputHandler.PendingCommand.Value == JobType.None);
        selection.SelectedReactive.Subscribe(SwitchPanel);
    }

    private void SwitchPanel(Selectable selectable) {
        if (selectable is SettlerSelectable) {
            _settlerPanel.gameObject.SetActive(true);
            _settlerPanel.SetData((AI.SettlerData)selectable.GetData(), Unselect);
            _infoPanelView.gameObject.SetActive(false);
        } else if (selectable is CommandTargetSelectable) {
            _settlerPanel.gameObject.SetActive(false);
            _infoPanelView.gameObject.SetActive(selectable != null);
            _infoPanelView.SetData((CommandTargetData)selectable.GetData());
        } else {
            _settlerPanel.gameObject.SetActive(false);
            _infoPanelView.gameObject.SetActive(false);
        }
    }

    public void Unselect() {
        _selection.Unselect();
    }

    public void Dispose() {
        SelectionEnabled?.Dispose();
    }
}