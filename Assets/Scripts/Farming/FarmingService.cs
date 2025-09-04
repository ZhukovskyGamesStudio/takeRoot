using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using Object = UnityEngine.Object;

public class FarmingService : IFarmingService, IUpdatable, IDisposable {
    private IUpdateService _updateService;
    private readonly IInputService _inputService;
    private readonly INetworkService _networkService;

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

    public FarmingService(IUpdateService updateService, IConfigsProvider configsProvider, IInputService inputService, INetworkService networkService) {
        _updateService = updateService;
        _inputService = inputService;
        _networkService = networkService;

        _farmingPlantConfigs = configsProvider.FarmingConfigs;
        inputService.OnSelectionEnd += OnSelectionEnd;
        FarmingPlots = GetAllPlots();

        _updateService.Register(this);
        
        /*foreach (var plot in FarmingPlots) {
            plot.Init();
        }*/
    }

    private void OnSelectionEnd(Rect selectedArea) {
        if (_selectedPlantToPlant) {
            foreach (var plot in FarmingPlots) {
                if (selectedArea.Contains(plot.transform.position) && plot.PlantType == FarmingPlantType.None) {
                    plot.ChangePlantServerRpc(_selectedPlantToPlant.PlantType);
                }
            }
        }

        if (_isCutting) {
            foreach (var plot in FarmingPlots) {
                if (selectedArea.Contains(plot.transform.position) && plot.PlantType != FarmingPlantType.None) {
                    plot.CutPlantServerRpc();
                }
            }
        }
    }

    public void Update() {
        //todo optimize
        FarmingPlots = GetAllPlots();
        if (_isCutting) {
            if (_inputService.GetKeyDown(KeyCode.Escape) || _inputService.GetMouseButtonDown(MouseButton.Right)) {
                _isCutting = false;
            }
        }

        if (_networkService.IsHost) {
            UpdateFarmingPlots();
        }
    }

    private void UpdateFarmingPlots() {
        foreach (var plot in FarmingPlots) {
            plot.ChangeWaterLevelServerRpc(-DryRate * Time.deltaTime);
            if (plot.PlantState == FarmingPlantState.Growing) {
                plot.ChangeGrow(GrowRate * Time.deltaTime);
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