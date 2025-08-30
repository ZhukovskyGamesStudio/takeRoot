using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using Object = UnityEngine.Object;

public class FarmingService : IFarmingService, IUpdatable, IDisposable {
    private IUpdateService _updateService;
    private readonly IInputService _inputService;
    public List<FarmingPlot> FarmingPlots = new List<FarmingPlot>();

    //список farming plant грядок

    //GetPlantThatNeedWatering
    //GetPlantsThatNeedHarvesting
    //GetPlantsThathNeedReplanting

    public float DryRate = 1 / 120f;
    public float GrowRate = 1 / 300f * 30;

    private List<FarmingPlantConfig> _farmingPlantConfigs;
    public List<FarmingPlantConfig> AvailableFarmingPlantConfigs => _farmingPlantConfigs;

    private bool _isCutting;
    private FarmingPlantConfig _selectedPlantToPlant;

    public FarmingService(IUpdateService updateService, IConfigsProvider configsProvider, IInputService inputService) {
        _updateService = updateService;
        _inputService = inputService;
        _updateService.Register(this);
        _farmingPlantConfigs = configsProvider.FarmingConfigs;
        inputService.OnSelectionEnd += OnSelectionEnd;
        var plots = GetAllPlots();
        foreach (var plot in plots) {
            plot.Init();
        }
    }

    private void OnSelectionEnd(Rect selectedArea) {
        if (_selectedPlantToPlant) {
            var plots = GetAllPlots();
            foreach (var plot in plots) {
                if (selectedArea.Contains(plot.transform.position) && plot.PlantType == FarmingPlantType.None) {
                    plot.ChangePlant(_selectedPlantToPlant);
                }
            }
        }

        if (_isCutting) {
            var plots = GetAllPlots();
            foreach (var plot in plots) {
                if (selectedArea.Contains(plot.transform.position) && plot.PlantType != FarmingPlantType.None) {
                    plot.CutPlant();
                }
            }
        }
    }

    public void Update() {
        //todo optimize
        var plots = GetAllPlots();

        foreach (var plot in plots) {
            plot.ChangeWaterLevel(-DryRate * Time.deltaTime);
            if (plot.PlantState == FarmingPlantState.Growing) {
                plot.ChangeGrow(GrowRate * Time.deltaTime);
            }
        }

        if (_isCutting) {
            if (_inputService.GetKeyDown(KeyCode.Escape) || _inputService.GetMouseButtonDown(MouseButton.Right)) {
                _isCutting = false;
            }
        }
    }

    public void SelectPlantToPlant(FarmingPlantConfig config) {
        _selectedPlantToPlant = config;
        _isCutting = false;
    }

    public void SelectCut() {
        _isCutting = true;
        _selectedPlantToPlant = null;
    }

    public void DeselectAll() {
        _isCutting = false;
        _selectedPlantToPlant = null;
    }

    private static FarmingPlot[] GetAllPlots() => Object.FindObjectsByType<FarmingPlot>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

    public void Dispose() {
        _updateService.Unregister(this);
    }
}