using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PanelsPresenter : IDisposable {
    private readonly PanelTogglesView _togglesView;
    private readonly BuildingsPanelView _buildingsPanelView;

    public PanelsPresenter(PanelTogglesView togglesView, PanelsView panelsView, ISelectionService selectionService) {
        _togglesView = togglesView;
        foreach (KeyValuePair<PanelType, Toggle> kvp in _togglesView.Toggles) {
            if (panelsView.Panels.TryGetValue(kvp.Key, out GameObject panel)) {
                kvp.Value.onValueChanged.AddListener(panel.SetActive);
            }
        }

        selectionService.SelectedReactive.Subscribe(CloseAllIfNoneSelected);
        CloseAllPanels();
    }

    public void CloseAllPanels() {
        foreach (KeyValuePair<PanelType, Toggle> kvp in _togglesView.Toggles) {
            kvp.Value.isOn = false;
        }
    }

    public void CloseAllIfNoneSelected(Selectable selectable) {
        if (selectable == null) {
            CloseAllPanels();
        }
    }

    public void Dispose() { }
}