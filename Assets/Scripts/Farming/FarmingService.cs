using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using Object = UnityEngine.Object;

public class FarmingService : IFarmingService, IUpdatable, IDisposable {
    private IUpdateService _updateService;
    private readonly IInputService _inputService;

    //список farming plant грядок

    //GetPlantThatNeedWatering
    //GetPlantsThatNeedHarvesting
    //GetPlantsThathNeedReplanting

    public float DryRate = 1 / 120f;
    public float GrowRate = 1 / 300f;

    private List<FarmingPlantConfig> _farmingPlantConfigs;
    public List<FarmingPlantConfig> AvailableFarmingPlantConfigs => _farmingPlantConfigs;
    public List<FarmingPlot> FarmingPlots { get; set; }= new List<FarmingPlot>();

    private bool _isCutting;
    private FarmingPlantConfig _selectedPlantToPlant;

    public FarmingService(IUpdateService updateService, IConfigsProvider configsProvider, IInputService inputService) {
        _updateService = updateService;
        _inputService = inputService;
        _updateService.Register(this);
        _farmingPlantConfigs = configsProvider.FarmingConfigs;
        inputService.OnSelectionEnd += OnSelectionEnd;
        FarmingPlots = GetAllPlots();
        /*foreach (var plot in FarmingPlots) {
            plot.Init();
        }*/
    }

    private void OnSelectionEnd(Rect selectedArea) {
        if (_selectedPlantToPlant) {
            foreach (var plot in FarmingPlots) {
                if (selectedArea.Contains(plot.transform.position) && plot.PlantType == FarmingPlantType.None) {
                    plot.ChangePlant(_selectedPlantToPlant);
                }
            }
        }

        if (_isCutting) {
            foreach (var plot in FarmingPlots) {
                if (selectedArea.Contains(plot.transform.position) && plot.PlantType != FarmingPlantType.None) {
                    plot.CutPlant();
                }
            }
        }
    }

    public void Update() {
        //todo optimize
        FarmingPlots = GetAllPlots();

        foreach (var plot in FarmingPlots) {
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

    private static List<FarmingPlot> GetAllPlots() => Object.FindObjectsByType<FarmingPlot>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();

    public void Dispose() {
        _updateService.Unregister(this);
    }
}