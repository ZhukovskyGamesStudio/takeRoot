using System;
using CodeBase.Services;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(WaterLevel))]
public class FarmingPlot : MonoBehaviour {
    [SerializeField]
    public float DryThreshold = 0.3f;

    [SerializeField]
    public float GrowThreshold = 1f;

    [SerializeField]
    private SpriteRenderer _plotView, _plantView;

    [SerializeField]
    private Sprite _wateredSprite, _drySprite;
    
    [SerializeField]
    private Color _blueprintColor = Color.blue;

    [HideInInspector]
    public FarmingPlantType PlantType;

    [HideInInspector]
    public FarmingPlantState PlantState;

    private WaterLevel _waterLevel;
    [HideInInspector]
    public float GrowingLevel;

    private FarmingPlantConfig _plantConfig;

    [HideInInspector]
    public AI.Settler Farmer;

    private void Start() {
        Init();
    }

    public void Init() {
        _waterLevel = GetComponent<WaterLevel>();
        _waterLevel.currentWater.Subscribe(OnWaterLevelChanged);
        PlantType = FarmingPlantType.None;
        PlantState = FarmingPlantState.None;
        _waterLevel.ChangeWater(Random.Range(0,1f));
        GrowingLevel = 0;
    }

    public void ChangePlant(FarmingPlantConfig plantConfig) {
        _plantConfig = plantConfig;
        PlantType = plantConfig.PlantType;
        PlantState = FarmingPlantState.WaitingForPlanting;

       
        GrowingLevel = 0;
        ChangeGrow(0);
        _plantView.color = _blueprintColor;
    }

    public void PlantFinished() {
        PlantState = FarmingPlantState.Growing;
        _plantView.color = Color.white;
    }

    public void CutPlant() {
        _plantConfig = null;
        PlantType = FarmingPlantType.None;
        PlantState = FarmingPlantState.None;
        GrowingLevel = 0;
        ChangeGrow(0);
    }

    public void ChangeWaterLevel(float amount) {
        if (_waterLevel == null) {
            _waterLevel = GetComponent<WaterLevel>();
        }
        _waterLevel.ChangeWater(amount);
    }

    private void OnWaterLevelChanged(float value) {
        _plotView.sprite = _waterLevel.currentWater.Value <= DryThreshold ? _drySprite : _wateredSprite;

        if (PlantState == FarmingPlantState.Growing && _waterLevel.currentWater.Value <= DryThreshold) {
            PlantState = FarmingPlantState.WaitingForWater;
        } else if (PlantState == FarmingPlantState.WaitingForWater && _waterLevel.currentWater.Value > DryThreshold) {
            PlantState = FarmingPlantState.Growing;
        }
    }

    public void ChangeGrow(float amount) {
        if (PlantState == FarmingPlantState.Growing) {
            GrowingLevel += amount;
            GrowingLevel = Mathf.Clamp01(GrowingLevel);
        }

        if (GrowingLevel >= GrowThreshold) {
            PlantState = FarmingPlantState.ReadyToHarvest;
        }

        _plantView.sprite = _plantConfig != null ? _plantConfig.GetGrowthSpriteByLevel(GrowingLevel) : null;
    }

    public void Harvest() {
        var resourceManager = ServiceLocator.Container.Single<IResourceManager>();
        foreach (var drop in _plantConfig.DropOnHarvest) {
            resourceManager.SpawnResource(transform.position, drop.type, drop.amount);
        }
        PlantState = FarmingPlantState.Growing;
        ChangeGrow(-1);
    }

    public bool NeedsWatering() => PlantType != FarmingPlantType.None && PlantState != FarmingPlantState.WaitingForWater;
    
    public bool NeedsPlanting() => PlantState == FarmingPlantState.WaitingForPlanting;
    public bool CanBeHarvested() => PlantState == FarmingPlantState.ReadyToHarvest;
}