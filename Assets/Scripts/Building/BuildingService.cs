using System.Collections.Generic;
using Settlers.Building;
using UnityEngine;

public class BuildingService : IBuildingService {
	private List<BuildingRecipeConfig> _buildingRecipeConfigs;
	private BuildingBlueprint _prefab;
 	private List<BuildingBlueprint> _blueprints = new List<BuildingBlueprint>();

    public bool IsEnabled { get; set; } = true;

	public BuildingService(BuildingsConfig buildingConfigs,BuildingsPanelView buildingsPanelView) {
		_buildingRecipeConfigs = buildingConfigs.recipeConfigs;
		_prefab = buildingConfigs.buildingBlueprintPrefab;
		buildingsPanelView.SetData(_buildingRecipeConfigs, CreateBuildingBlueprint);
	}
	
	public BuildingBlueprint GetBuildingBlueprintWithJob() {
		foreach (BuildingBlueprint blueprint in _blueprints) {
			if (!blueprint.IsPlaced) continue;
			
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
}