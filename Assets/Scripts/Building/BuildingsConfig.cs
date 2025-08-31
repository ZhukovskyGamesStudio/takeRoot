using System.Collections.Generic;
using UnityEngine;

	[CreateAssetMenu(fileName = "BuildingsConfig", menuName = "Scriptable Objects/BuildingsConfig", order = 0)]
	public class BuildingsConfig : ScriptableObject {
		public BuildingBlueprint buildingBlueprintPrefab;
		public List<BuildingRecipeConfig> recipeConfigs;
	}
