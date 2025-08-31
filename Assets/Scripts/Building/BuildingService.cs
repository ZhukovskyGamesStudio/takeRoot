using System;
using System.Collections.Generic;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using Object = UnityEngine.Object;

public class BuildingService : IBuildingService, IUpdatable {
	private List<BuildingRecipeConfig> _buildingRecipeConfigs;
	private BuildingBlueprint _prefab;
	private List<BuildingBlueprint> _blueprints = new List<BuildingBlueprint>();
	private readonly IGridService _map;
	private readonly IPhysicsService _physics;
	private readonly IUpdateService _update;
	private readonly IInputService _input;

	public bool IsEnabled { get; set; } = true;

	public BuildingService(IConfigsProvider configsProvider,BuildingsPanelView buildingsPanelView) {
		_map = ServiceLocator.Container.Single<IGridService>();
		_physics = ServiceLocator.Container.Single<IPhysicsService>();
		_update = ServiceLocator.Container.Single<IUpdateService>();
		_input = ServiceLocator.Container.Single<IInputService>();
		_buildingRecipeConfigs = configsProvider.BuildingsConfig.recipeConfigs;
		_prefab = configsProvider.BuildingsConfig.buildingBlueprintPrefab;
		buildingsPanelView.SetData(_buildingRecipeConfigs, CreateBuildingBlueprint);
		_update.Register(this);
	}
	
	public BuildingBlueprint GetBuildingBlueprintWithTransportJob() {
		foreach (BuildingBlueprint blueprint in _blueprints) {
			if (blueprint.WasBuilded) continue;
			if (!blueprint.IsPlaced) continue;
			if (blueprint.CanBuild()) continue;
			if (_map.IsOccupiedPos(blueprint.InteractionPos.position)) continue; //TODO: make multiply interact pos around blueprint
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

	public void CreateBuildingBlueprint(BuildingRecipeConfig recipeConfig) {
		if (!IsEnabled) return;
		BuildingBlueprint blueprint = Object.Instantiate<BuildingBlueprint>(_prefab);
		IsEnabled = false;
		blueprint.Init(recipeConfig);
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
		if (_input.GetMouseButtonDown(MouseButton.Right)) {
			var blueprint = _physics.Raycast<BuildingBlueprint>(_input.GetWorldMousePosition(), Vector2.zero);
			if (blueprint != null) {
				blueprint.CancelPlacement();
			}
		}
	}
}