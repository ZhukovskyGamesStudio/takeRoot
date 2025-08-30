using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class FarmingService : IFarmingService, IUpdatable, IDisposable {
    private IUpdateService _updateService;
    public List<FarmingPlot> FarmingPlots = new List<FarmingPlot>();

    //список farming plant грядок

    //GetPlantThatNeedWatering
    //GetPlantsThatNeedHarvesting
    //GetPlantsThathNeedReplanting

    public float DryRate = 1 / 120f;
    public float GrowRate = 1 / 300f;

    private List<FarmingPlantConfig> _farmingPlantConfigs;
    
    public List<FarmingPlantConfig> AvailableFarmingPlantConfigs => _farmingPlantConfigs;

    public FarmingService(IUpdateService updateService, IConfigsProvider configsProvider) {
        _updateService = updateService;
        _updateService.Register(this);
        _farmingPlantConfigs = configsProvider.FarmingConfigs;
        var plots = GetAllPlots();
        foreach (var plot in plots) {
            plot.Init();
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
    }

    private static FarmingPlot[] GetAllPlots() => Object.FindObjectsByType<FarmingPlot>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

    public void Dispose() {
        _updateService.Unregister(this);
    }
}