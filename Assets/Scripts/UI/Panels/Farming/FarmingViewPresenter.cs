using UnityEngine;

public class FarmingViewPresenter {
    private FarmingPanelView _view;
    private readonly IFarmingService _service;
    private FarmingPlantConfig _selectedPlantConfig;

    public FarmingViewPresenter(FarmingPanelView view, IFarmingService service) {
        _view = view;
        _service = service;
        _view.SetData(_service.AvailableFarmingPlantConfigs, OnPlant, OnCut, DeselectAll);
    }

    private void OnCut() {
        _service.SelectCut();
        Debug.Log("Farming cut selected");
    }

    private void OnPlant(FarmingPlantConfig config) {
        _selectedPlantConfig = config;
        _service.SelectPlantToPlant(config);
        Debug.Log($"Farming plant {config.MainData.Name} selected");
    }

    private void DeselectAll() {
        _service.DeselectAll();
        Debug.Log("Farming deselected");
    }
}