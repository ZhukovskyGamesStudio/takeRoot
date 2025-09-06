using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class BuildingService : IBuildingService, IUpdatable {
    private readonly INetworkService _networkService;
    private readonly IJobCommandsInputHandlerService _jobs;
    private List<BuildingRecipeConfig> _buildingRecipeConfigs;
    private BuildingBlueprint _prefab;
    private List<BuildingBlueprint> _blueprints = new List<BuildingBlueprint>();
    private readonly IGridService _map;
    private readonly IPhysicsService _physics;
    private readonly IUpdateService _update;
    private readonly IInputService _input;

    public bool IsEnabled { get; set; } = true;

    public BuildingService(IConfigsProvider configsProvider, BuildingsPanelView buildingsPanelView, INetworkService networkService, IJobCommandsInputHandlerService jobs) {
        _networkService = networkService;
        _jobs = jobs;
        _map = ServiceLocator.Container.Single<IGridService>();
        _physics = ServiceLocator.Container.Single<IPhysicsService>();
        _update = ServiceLocator.Container.Single<IUpdateService>();
        _input = ServiceLocator.Container.Single<IInputService>();
        _buildingRecipeConfigs = configsProvider.BuildingsBlueprintsConfigs;
        _prefab = configsProvider.BuildingsConfig.buildingBlueprintPrefab;
        buildingsPanelView.SetData(_buildingRecipeConfigs, CreateBuildingBlueprint);
        _update.Register(this);
    }

    public BuildingBlueprint GetBuildingBlueprintWithTransportJob() {
        foreach (BuildingBlueprint blueprint in _blueprints) {
            if (blueprint.WasBuilded) continue;
            if (!blueprint.IsPlaced) continue;
            if (blueprint.CanBuild()) continue;
            if (blueprint.GetRequiredResource() == ResourceType.None) continue;
            if (blueprint.InteractionPos == null) continue;
            return blueprint;
        }

        return null;
    }

    public BuildingBlueprint GetBuildingBlueprintWithBuildJob() {
        foreach (BuildingBlueprint blueprint in _blueprints) {
            if (blueprint.CanBuild() && blueprint.Builder == null)
                return blueprint;
        }

        return null;
    }

    public void CreateBuildingBlueprint(string buildingName, Race placedByRace) {
        Debug.Log("CreateBuildingBlueprint");
        if (!IsEnabled) {
            Debug.Log("CreateBuildingBlueprint cancelled, service not enabled");
            return;
        }

        if (!_networkService.IsHost) {
            Debug.Log("CreateBuildingBlueprint cancelled, not host");
            return;
        }

        BuildingBlueprint blueprint = _networkService.InstantiateAndSpawn(_prefab);
        IsEnabled = false;
        blueprint.InitClientRpc(buildingName, placedByRace);
    }

    public void PlaceBlueprint(BuildingBlueprint blueprint) {
        _blueprints.Add(blueprint);
        IsEnabled = true;
    }

    public void CancelBlueprint(BuildingBlueprint blueprint) {
        _blueprints.Remove(blueprint);
        IsEnabled = true;
    }

    public void Build(BuildingBlueprint blueprint) {
        _blueprints.Remove(blueprint);
    }

    public void Update() {
        //TODO add cancel via cancel command
        if (_input.GetMouseButtonDown(MouseButton.Left) && _jobs.PendingCommand.Value == JobType.Cancel) {
            var blueprint = _physics.Raycast<BuildingBlueprint>(_input.GetWorldMousePosition(), Vector2.zero);
            if (blueprint != null) {
                blueprint.CancelPlacement();
            }
        }
    }

    public void Dispose() {
        _update.Unregister(this);
    }
}