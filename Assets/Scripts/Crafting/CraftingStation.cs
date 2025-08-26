using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using Settlers.Crafting;
using UnityEngine;

public class CraftingStation : MonoBehaviour {
    public Dictionary<ResourceType, int> RequiredResources;
    public Dictionary<ResourceType, int> ReservedRequiredResources;
    public Dictionary<ResourceType, int> ResourceStorage;
    public List<CraftingRecipeConfig> AvailableCraftingRecipes;
    public Dictionary<string, int> RecipesToCraft;
    private ICraftingService _craftingService;

    private void Start() {
        _craftingService = ServiceLocator.Container.Single<ICraftingService>();
        _craftingService.AddCraftingStation(this);
        RequiredResources = new Dictionary<ResourceType, int>();
        ResourceStorage = new Dictionary<ResourceType, int>();
        ReservedRequiredResources = new Dictionary<ResourceType, int>();
        RecipesToCraft = new Dictionary<string, int>();
        foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
            if (type == ResourceType.None) {
                continue;
            }
            ReservedRequiredResources[type] = 0;
            RequiredResources[type] = 0;
            ResourceStorage[type] = 0;
        }
        foreach (CraftingRecipeConfig config in AvailableCraftingRecipes) {
            RecipesToCraft[config.RecipeUid] = 0;
        }
        //RequiredResources[ResourceType.Planks] = 5;
    }

    public ResourceType GetRequiredResource() {
        foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
            if (type == ResourceType.None) {
                continue;
            }
            var amount = RequiredResources[type] - ReservedRequiredResources[type] - ResourceStorage[type];

            if (amount > 0) {
                return type;
            }
        }
        return ResourceType.None;
    }

    public void StoreResource(ResourceType type, int amount) {
        ResourceStorage[type] += amount;
        ReservedRequiredResources[type] -= amount;
    }

    public void AddRecipe(string recipeUid) {
        var recipe = AvailableCraftingRecipes.FirstOrDefault(r => r.RecipeUid == recipeUid);
        if (recipe == null) {
            Debug.LogError("Recipe UID: " + recipeUid + " not found!");
            return;
        }
        RecipesToCraft[recipeUid]++;
        foreach (ResourceData resource in recipe.RequiredResources) {
            RequiredResources[resource.ResourceType] += resource.Amount;
        }
    }

    public void RemoveRecipe(string recipeUid) {
        if (RecipesToCraft[recipeUid] == 0) return;
        var recipe = AvailableCraftingRecipes.FirstOrDefault(r => r.RecipeUid == recipeUid);
        if (recipe == null) {
            Debug.LogError("Recipe UID: " + recipeUid + " not found!");
            return;
        }
        RecipesToCraft[recipeUid]--;
        foreach (ResourceData resource in recipe.RequiredResources) {
            RequiredResources[resource.ResourceType] -= resource.Amount;
        }
        
    }
}