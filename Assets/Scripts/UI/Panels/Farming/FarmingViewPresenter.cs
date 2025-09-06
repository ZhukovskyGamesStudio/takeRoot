using System.Linq;
using CodeBase.Services;
using UnityEngine;

public class FarmingViewPresenter {
    private FarmingPanelView _view;
    private readonly IFarmingService _service;
    private FarmingPlantConfig _selectedPlantConfig;
    private readonly ICursorService _cursor;

    public FarmingViewPresenter(FarmingPanelView view, IFarmingService service, ICursorService cursor) {
        _view = view;
        _service = service;
        _cursor = cursor;
        _view.SetData(_service.AvailableFarmingPlantConfigs, OnPlant, OnCut, DeselectAll);
    }

    private void OnCut() {
        _service.SelectCut();
        _cursor.SetCursorIcon(_view.SickleIcon);
        Debug.Log("Farming cut selected");
    }

    private void OnPlant(FarmingPlantConfig config) {
        _selectedPlantConfig = config;
        _service.SelectPlantToPlant(config);
        _cursor.SetCursorIcon(config.Sprites.First());
        Debug.Log($"Farming plant {config.MainData.Name} selected");
    }

    private void DeselectAll() {
        _service.DeselectAll();
        _cursor.SetDefaultCursorIcon();
        Debug.Log("Farming deselected");
    }
}