using System;
using System.Collections.Generic;
using System.Linq;
using Settlers.Crafting;
using UnityEngine;

[Serializable]
public class CraftingStationData {
    public List<CraftingRecipeConfig> AvailableCraftingRecipes;
    public CraftingStation Station;

    [HideInInspector]
    public AYellowpaper.SerializedCollections.SerializedDictionary<ResourceType, int> ResourceStorage;

    [HideInInspector]
    public Dictionary<ResourceType, int> RequiredResources;

    [HideInInspector]
    public Dictionary<ResourceType, int> RecipesToCraft;

    [HideInInspector]
    public CraftingRecipeConfig CurrentRecipe;

    [HideInInspector]
    public int CurrentRecipeCraftingPoints;

    public void Init(CraftingStation station) {
        Station = station;
        ResourceStorage = new AYellowpaper.SerializedCollections.SerializedDictionary<ResourceType, int>();
        RequiredResources = new Dictionary<ResourceType, int>();
        RecipesToCraft = new Dictionary<ResourceType, int>();
        foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
            if (type == ResourceType.None) {
                continue;
            }

            RequiredResources[type] = 0;
            ResourceStorage[type] = 0;
        }

        foreach (CraftingRecipeConfig config in AvailableCraftingRecipes) {
            RecipesToCraft[config.ResultingResource.ResourceType] = 0;
        }
    }
    
    public void AddRecipe(ResourceType recipeRes) {
        var recipe = AvailableCraftingRecipes.FirstOrDefault(r => r.ResultingResource.ResourceType == recipeRes);
        if (recipe == null) {
            Debug.LogError("Recipe UID: " + recipeRes + " not found!");
            return;
        }

        RecipesToCraft[recipeRes]++;
        foreach (ResourceData resource in recipe.RequiredResources) {
            RequiredResources[resource.ResourceType] += resource.Amount;
        }
    }
    
    public void RemoveRecipe(ResourceType recipeRes) {
        if (RecipesToCraft[recipeRes] == 0) {
            return;
        }
        var recipe = AvailableCraftingRecipes.FirstOrDefault(r => r.ResultingResource.ResourceType == recipeRes);
        if (recipe == null) {
            Debug.LogError("Recipe UID: " + recipeRes + " not found!");
            return;
        }

        RecipesToCraft[recipeRes]--;
        if (CurrentRecipe.ResultingResource.ResourceType == recipeRes && RecipesToCraft[recipeRes] == 0) {
            CurrentRecipe = null;
        }

        foreach (ResourceData resource in recipe.RequiredResources) {
            RequiredResources[resource.ResourceType] -= resource.Amount;
        }
    }
}