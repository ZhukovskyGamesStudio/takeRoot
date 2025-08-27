using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using Settlers.Crafting;
using UnityEngine;

public class CraftingStation : MonoBehaviour {
    [field: SerializeField]
    public CraftingStationData StationData { get; private set; }

    [HideInInspector]
    public Dictionary<ResourceType, int> ReservedRequiredResources;

    [HideInInspector]
    public AYellowpaper.SerializedCollections.SerializedDictionary<Race, AI.Settler> Crafters = new();

    [HideInInspector]
    public List<Transform> InteractPos = new List<Transform>(2);

    private ICraftingService _craftingService;

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

        StationData.Init();
    }

    public ResourceType GetRequiredResource() {
        foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
            if (type == ResourceType.None) {
                continue;
            }

            var amount = StationData.RequiredResources[type] - ReservedRequiredResources[type] - StationData.ResourceStorage[type];

            if (amount > 0) {
                return type;
            }
        }

        return ResourceType.None;
    }

    public void StoreResource(ResourceType type, int amount) {
        StationData.ResourceStorage[type] += amount;
        ReservedRequiredResources[type] -= amount;
    }

    public void AddRecipe(string recipeUid) {
        var recipe = StationData.AvailableCraftingRecipes.FirstOrDefault(r => r.RecipeUid == recipeUid);
        if (recipe == null) {
            Debug.LogError("Recipe UID: " + recipeUid + " not found!");
            return;
        }

        StationData.RecipesToCraft[recipeUid]++;
        foreach (ResourceData resource in recipe.RequiredResources) {
            StationData.RequiredResources[resource.ResourceType] += resource.Amount;
        }
    }

    public void RemoveRecipe(string recipeUid) {
        if (StationData.RecipesToCraft[recipeUid] == 0) return;
        var recipe = StationData.AvailableCraftingRecipes.FirstOrDefault(r => r.RecipeUid == recipeUid);
        if (recipe == null) {
            Debug.LogError("Recipe UID: " + recipeUid + " not found!");
            return;
        }

        StationData.RecipesToCraft[recipeUid]--;
        if (StationData.CurrentRecipe.RecipeUid == recipeUid && StationData.RecipesToCraft[recipeUid] == 0) {
            StationData.CurrentRecipe = null;
        }

        foreach (ResourceData resource in recipe.RequiredResources) {
            StationData.RequiredResources[resource.ResourceType] -= resource.Amount;
        }
    }

    public bool CanCraft() {
        PickNewRecipe();
        var canCraft = false;
        foreach (CraftingRecipeConfig config in StationData.AvailableCraftingRecipes) {
            if (StationData.RecipesToCraft[config.RecipeUid] == 0) continue;
            foreach (ResourceData resource in config.RequiredResources) {
                if (StationData.ResourceStorage[resource.ResourceType] < resource.Amount) {
                    canCraft = false;
                    break;
                }

                canCraft = true;
            }
        }

        return canCraft;
    }

    public void Craft() {
        StationData.CurrentRecipeCraftingPoints++;
        if (StationData.CurrentRecipe.CraftingPoints == StationData.CurrentRecipeCraftingPoints) {
            CraftResource();
            PickNewRecipe();
        }
    }

    private void CraftResource() {
        var resource = StationData.CurrentRecipe.ResultingResource;
        foreach (ResourceData requiredResources in StationData.CurrentRecipe.RequiredResources) {
            StationData.ResourceStorage[requiredResources.ResourceType] -= requiredResources.Amount;
            StationData.RequiredResources[requiredResources.ResourceType] -= requiredResources.Amount;
        }

        StationData.CurrentRecipeCraftingPoints = 0;
        StationData.CurrentRecipe = null;
        Debug.Log($"Crafted {resource.ResourceType}");
    }

    private void PickNewRecipe() {
        CraftingRecipeConfig recipe = null;
        foreach (CraftingRecipeConfig config in StationData.AvailableCraftingRecipes) {
            if (StationData.RecipesToCraft[config.RecipeUid] == 0) continue;
            foreach (ResourceData resource in config.RequiredResources) {
                if (StationData.ResourceStorage[resource.ResourceType] < resource.Amount) {
                    break;
                }

                StationData.CurrentRecipe = StationData.AvailableCraftingRecipes.FirstOrDefault(r => r.RecipeUid == config.RecipeUid);
                return;
            }
        }

        ;
    }
}