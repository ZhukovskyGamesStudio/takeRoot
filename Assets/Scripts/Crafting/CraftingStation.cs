using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using Settlers.Crafting;
using UnityEngine;

public class CraftingStation : MonoBehaviour {
	public CraftingStationData stationData;
	
	public Dictionary<ResourceType, int> ReservedRequiredResources;
	private ICraftingService _craftingService;
	public AYellowpaper.SerializedCollections.SerializedDictionary<Race, AI.Settler> Crafters = new();
	public List<Transform> InteractPos = new List<Transform>(2);
    
    
	private void Start() {
		_craftingService = ServiceLocator.Container.Single<ICraftingService>();
		_craftingService.AddCraftingStation(this);
		ReservedRequiredResources = new Dictionary<ResourceType, int>();
		foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
			if (type == ResourceType.None) {
				continue;
			}
			ReservedRequiredResources[type] = 0;
		}
		stationData = new CraftingStationData();
		stationData.Init();
	}

	public ResourceType GetRequiredResource() {
		foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
			if (type == ResourceType.None) {
				continue;
			}
			var amount = stationData.RequiredResources[type] - ReservedRequiredResources[type] - stationData.ResourceStorage[type];

			if (amount > 0) {
				return type;
			}
		}
		return ResourceType.None;
	}

	public void StoreResource(ResourceType type, int amount) {
		stationData.ResourceStorage[type] += amount;
		ReservedRequiredResources[type] -= amount;
	}

	public void AddRecipe(string recipeUid) {
		var recipe = stationData.AvailableCraftingRecipes.FirstOrDefault(r => r.RecipeUid == recipeUid);
		if (recipe == null) {
			Debug.LogError("Recipe UID: " + recipeUid + " not found!");
			return;
		}
		stationData.RecipesToCraft[recipeUid]++;
		foreach (ResourceData resource in recipe.RequiredResources) {
			stationData.RequiredResources[resource.ResourceType] += resource.Amount;
		}
	}

	public void RemoveRecipe(string recipeUid) {
		if (stationData.RecipesToCraft[recipeUid] == 0) return;
		var recipe = stationData.AvailableCraftingRecipes.FirstOrDefault(r => r.RecipeUid == recipeUid);
		if (recipe == null) {
			Debug.LogError("Recipe UID: " + recipeUid + " not found!");
			return;
		}
		stationData.RecipesToCraft[recipeUid]--;
		if (stationData.CurrentRecipe.RecipeUid == recipeUid && stationData.RecipesToCraft[recipeUid] == 0) {
			stationData.CurrentRecipe = null;
		}
		foreach (ResourceData resource in recipe.RequiredResources) {
			stationData.RequiredResources[resource.ResourceType] -= resource.Amount;
		}
	}

	public bool CanCraft() {
		PickNewRecipe();
		var canCraft = false;
		foreach (CraftingRecipeConfig config in stationData.AvailableCraftingRecipes) {
			if (stationData.RecipesToCraft[config.RecipeUid] == 0) continue;
			foreach (ResourceData resource in config.RequiredResources) {
				if (stationData.ResourceStorage[resource.ResourceType] < resource.Amount) {
					canCraft = false;
					break;
				}
				canCraft = true;
			}
		}
		return canCraft;
	}

	public void Craft() {
		stationData.CurrentRecipeCraftingPoints++;
		if (stationData.CurrentRecipe.CraftingPoints == stationData.CurrentRecipeCraftingPoints) {
			CraftResource();
			PickNewRecipe();
		}
	}

	private void CraftResource() {
		var resource = stationData.CurrentRecipe.ResultingResource;
		foreach (ResourceData requiredResources in stationData.CurrentRecipe.RequiredResources) {
			stationData.ResourceStorage[requiredResources.ResourceType] -= requiredResources.Amount;
			stationData.RequiredResources[requiredResources.ResourceType] -= requiredResources.Amount;
		}

		stationData.CurrentRecipeCraftingPoints = 0;
		stationData.CurrentRecipe = null;
		Debug.Log($"Crafted {resource.ResourceType}");
	}

	private void PickNewRecipe() {
		CraftingRecipeConfig recipe = null;
		foreach (CraftingRecipeConfig config in stationData.AvailableCraftingRecipes) {
			if (stationData.RecipesToCraft[config.RecipeUid] == 0) continue;
			foreach (ResourceData resource in config.RequiredResources) {
				if (stationData.ResourceStorage[resource.ResourceType] < resource.Amount) {
					break;
				}
				stationData.CurrentRecipe = stationData.AvailableCraftingRecipes.FirstOrDefault(r => r.RecipeUid == config.RecipeUid);
				return;
			}
		} ;
	}
}