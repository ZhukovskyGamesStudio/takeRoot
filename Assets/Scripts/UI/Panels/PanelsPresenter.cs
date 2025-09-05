using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PanelsPresenter : IDisposable {
	private readonly PanelTogglesView _togglesView;
	private readonly PanelsView _panelsView;
	private readonly INetworkService _networkService;
	private readonly IResearchService _researches;
	private readonly BuildingsPanelView _buildingsPanelView;

	public PanelsPresenter(PanelTogglesView togglesView, PanelsView panelsView, ISelectionService selectionService, INetworkService networkService, IResearchService researches) {
		_togglesView = togglesView;
		_panelsView = panelsView;
		_networkService = networkService;
		_researches = researches;
		foreach (KeyValuePair<PanelType, Toggle> kvp in _togglesView.Toggles) {
			if (panelsView.Panels.TryGetValue(kvp.Key, out GameObject panel)) {
				kvp.Value.onValueChanged.AddListener(panel.SetActive);
			}
		}

		_researches.OnResearchFinished += UpdatePanels;
		UpdatePanels();
        
        
		//bool isPlants = _networkService.MyRace.Value == Race.Plants;
		// _togglesView.Toggles[PanelType.Farming].gameObject.SetActive(isPlants);
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

	public void UpdatePanels() {
		foreach (KeyValuePair<PanelType, Toggle> kvp in _togglesView.Toggles) {
			if (_panelsView.Panels.TryGetValue(kvp.Key, out GameObject panel)) {
				var toggle = _togglesView.ToggleData[kvp.Key];
				if ((toggle.ResearchRequirement == Research.None ||
				     _researches.WasResearched(toggle.ResearchRequirement)) && (toggle.RaceRequirement == Race.Both || _networkService.MyRace.Value == toggle.RaceRequirement))
					kvp.Value.gameObject.SetActive(true);
				else
					kvp.Value.gameObject.SetActive(false);
			}
		}
	}
    
	public void Dispose() { }
}