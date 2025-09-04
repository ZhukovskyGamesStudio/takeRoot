using System;
using UniRx;

public class SelectionServicePresenter : IDisposable, IUpdatable {
    private readonly ISelectionService _selection;
    private readonly INetworkService _networkService;

    public ReactiveProperty<bool> SelectionEnabled;
    private InfoPanelView _infoPanelView;
    private SettlerInfoPanel _settlerPanel;

    public SelectionServicePresenter(InfoPanelView infoPanelView, SettlerInfoPanel settlerPanel,
        IJobCommandsInputHandlerService commandInputHandler, ISelectionService selection, IUpdateService updateService, INetworkService networkService) {
        _infoPanelView = infoPanelView;
        _settlerPanel = settlerPanel;
        _selection = selection;
        _networkService = networkService;
        SelectionEnabled = _selection.IsEnabled;
        commandInputHandler.PendingCommand.Subscribe(_ => SelectionEnabled.Value = commandInputHandler.PendingCommand.Value == JobType.None);
        selection.SelectedReactive.Subscribe(SwitchPanel);
        updateService.Register(this);
    }

    private void SwitchPanel(Selectable selectable) {
        if (selectable is SettlerSelectable) {
            var data = selectable.GetData(_networkService.MyRace.Value);
            if (data is AI.SettlerData settlerData) {
                _settlerPanel.gameObject.SetActive(true);
                _infoPanelView.gameObject.SetActive(false);
                _settlerPanel.SetData(settlerData, Unselect);
            } else {
                _settlerPanel.gameObject.SetActive(false);
                _infoPanelView.gameObject.SetActive(true);
                _infoPanelView.SetData((InfoDataCombined)data);
            }
          
        } else if (selectable is CommandTargetSelectable) {
            _settlerPanel.gameObject.SetActive(false);
            _infoPanelView.gameObject.SetActive(selectable != null);
            _infoPanelView.SetData((InfoDataCombined)selectable.GetData(_networkService.MyRace.Value));
        } else {
            _settlerPanel.gameObject.SetActive(false);
            _infoPanelView.gameObject.SetActive(false);
        }
    }

    public void Update() {
        if(_settlerPanel.gameObject.activeSelf) _settlerPanel.UpdateData();
    }

    public void Unselect() {
        _selection.Unselect();
    }

    public void Dispose() {
        SelectionEnabled?.Dispose();
    }
}