using System;
using System.Collections.Generic;
using CodeBase.Services;
using Settlers.Building;
using Object = UnityEngine.Object;

public class BuildingService : IBuildingService {
	private List<BuildingRecipeConfig> _buildingRecipeConfigs;
	private BuildingBlueprint _prefab;
 	private List<BuildingBlueprint> _blueprints = new List<BuildingBlueprint>();
    private readonly IGridService _map;

    public bool IsEnabled { get; set; } = true;

	public BuildingService(BuildingsConfig buildingConfigs,BuildingsPanelView buildingsPanelView) {
		_map = ServiceLocator.Container.Single<IGridService>();
		_buildingRecipeConfigs = buildingConfigs.recipeConfigs;
		_prefab = buildingConfigs.buildingBlueprintPrefab;
		buildingsPanelView.SetData(_buildingRecipeConfigs, CreateBuildingBlueprint);
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
		IsEnabled = true;
	}
	public void Build(BuildingBlueprint blueprint) {
		_blueprints.Remove(blueprint);
	}
}