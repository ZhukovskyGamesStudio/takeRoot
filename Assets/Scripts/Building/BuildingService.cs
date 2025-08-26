using System.Collections.Generic;
using Settlers.Building;
using UnityEngine;

public class BuildingService : IBuildingService {
	private List<BuildingRecipeConfig> _buildingRecipeConfigs;
	private BuildingBlueprint _prefab;
 	private List<BuildingBlueprint> _blueprints = new List<BuildingBlueprint>();

	public BuildingService(BuildingsConfig buildingConfigs,BuildingsPanelView buildingsPanelView) {
		_buildingRecipeConfigs = buildingConfigs.recipeConfigs;
		_prefab = buildingConfigs.buildingBlueprintPrefab;
		buildingsPanelView.SetData(_buildingRecipeConfigs, CreateBuilding);
	}
	
	public BuildingBlueprint GetBuildingBlueprintWithJob() {
		foreach (BuildingBlueprint blueprint in _blueprints) {
			if (!blueprint.IsPlaced) continue;
			
		}

		return null;
	}

	public void CreateBuilding(BuildingRecipeConfig recipeConfig) {
		BuildingBlueprint blueprint = Object.Instantiate<BuildingBlueprint>(_prefab);
		blueprint.Init(recipeConfig);
	}
}