using System.Linq;
using CodeBase.Services;
using UniRx;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(WaterLevel))]
public class FarmingPlot : NetworkBehaviour {
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
        if (IsHost) {
            Init();
            InitClientRpc();

            _waterLevel.ChangeWaterServerRpc(0);
            _waterLevel.CurrentWater.Subscribe(OnWaterLevelChanged);
        }
    }

    [ClientRpc]
    private void InitClientRpc() {
        Init();
    }

    private void Init() {
        _waterLevel = GetComponent<WaterLevel>();
        PlantType = FarmingPlantType.None;
        PlantState = FarmingPlantState.None;
        GrowingLevel = 0;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangePlantServerRpc(FarmingPlantType type) {
        _plantConfig = ServiceLocator.Container.Single<IConfigsProvider>().FarmingConfigs.First(c => c.PlantType == type);
        PlantType = type;
        PlantState = FarmingPlantState.WaitingForPlanting;

        GrowingLevel = 0;

        _plantView.color = _blueprintColor;
        SyncPlantClientRpc(PlantType, PlantState);
        ChangeGrow(0);
    }

    [ClientRpc]
    private void SyncPlantClientRpc(FarmingPlantType type, FarmingPlantState state) {
        if (type == FarmingPlantType.None) {
            _plantConfig = null;
        } else {
            _plantConfig = ServiceLocator.Container.Single<IConfigsProvider>().FarmingConfigs.First(c => c.PlantType == type);
        }

        PlantType = type;
        PlantState = state;
    }

    public void PlantFinished() {
        PlantState = FarmingPlantState.Growing;
        SyncGrowClientRpc(GrowingLevel, PlantState);
    }

    [ServerRpc(RequireOwnership = false)]
    public void CutPlantServerRpc() {
        _plantConfig = null;
        PlantType = FarmingPlantType.None;
        PlantState = FarmingPlantState.None;
        GrowingLevel = 0;
        ChangeGrow(0);
    }

    [ServerRpc]
    public void ChangeWaterLevelServerRpc(float amount) {
        if (_waterLevel == null) {
            _waterLevel = GetComponent<WaterLevel>();
        }

        _waterLevel.ChangeWaterServerRpc(amount);
    }

    private void OnWaterLevelChanged(float value) {
        UpdatePlotDrySprite();

        if (PlantState == FarmingPlantState.Growing && _waterLevel.CurrentWater.Value <= DryThreshold) {
            PlantState = FarmingPlantState.WaitingForWater;
        } else if (PlantState == FarmingPlantState.WaitingForWater && _waterLevel.CurrentWater.Value > DryThreshold) {
            PlantState = FarmingPlantState.Growing;
        }

        SyncWaterLevelChangeClientRpc();
    }

    private void UpdatePlotDrySprite() {
        _plotView.sprite = _waterLevel.CurrentWater.Value <= DryThreshold ? _drySprite : _wateredSprite;
    }

    [ClientRpc]
    private void SyncWaterLevelChangeClientRpc() {
        UpdatePlotDrySprite();
    }

    public void ChangeGrow(float amount) {
        if (PlantState == FarmingPlantState.Growing) {
            GrowingLevel += amount;
            GrowingLevel = Mathf.Clamp01(GrowingLevel);
        }

        if (GrowingLevel >= GrowThreshold) {
            PlantState = FarmingPlantState.ReadyToHarvest;
        }

        UpdatePlantView();
        SyncGrowClientRpc(GrowingLevel, PlantState);
    }

    private void UpdatePlantView() {
        _plantView.sprite = _plantConfig != null ? _plantConfig.GetGrowthSpriteByLevel(GrowingLevel) : null;
        _plantView.color = PlantState == FarmingPlantState.WaitingForPlanting ? _blueprintColor : Color.white;
    }

    [ClientRpc]
    private void SyncGrowClientRpc(float growLevel, FarmingPlantState state) {
        GrowingLevel = growLevel;
        PlantState = state;
        UpdatePlantView();
    }

    [ServerRpc]
    public void HarvestServerRpc() {
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