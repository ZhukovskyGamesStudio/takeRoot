using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelsPresenter : IDisposable {
    private readonly PanelTogglesView _togglesView;
    private readonly BuildingsPanelView _buildingsPanelView;

    public PanelsPresenter(PanelTogglesView togglesView, PanelsView panelsView) {
        _togglesView = togglesView;
        foreach (KeyValuePair<PanelType, Toggle> kvp in _togglesView.Toggles) {
            if (panelsView.Panels.TryGetValue(kvp.Key, out GameObject panel)) {
                kvp.Value.onValueChanged.AddListener(panel.SetActive);
            }
        }

        CloseAllPanels();
    }

    public void CloseAllPanels() {
        foreach (KeyValuePair<PanelType, Toggle> kvp in _togglesView.Toggles) {
            kvp.Value.isOn = false;
        }
    }

    public void Dispose() { }
}