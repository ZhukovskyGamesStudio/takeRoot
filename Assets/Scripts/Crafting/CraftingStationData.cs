using System;
using System.Collections.Generic;
using Settlers.Crafting;
using UnityEngine;

[Serializable]
public class CraftingStationData {
    public List<CraftingRecipeConfig> AvailableCraftingRecipes;

    [HideInInspector]
    public AYellowpaper.SerializedCollections.SerializedDictionary<ResourceType, int> ResourceStorage;

    [HideInInspector]
    public Dictionary<ResourceType, int> RequiredResources;

    [HideInInspector]
    public Dictionary<string, int> RecipesToCraft;

    [HideInInspector]
    public CraftingRecipeConfig CurrentRecipe;

    [HideInInspector]
    public int CurrentRecipeCraftingPoints;

    public void Init() {
        ResourceStorage = new AYellowpaper.SerializedCollections.SerializedDictionary<ResourceType, int>();
        RequiredResources = new Dictionary<ResourceType, int>();
        RecipesToCraft = new Dictionary<string, int>();
        foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
            if (type == ResourceType.None) {
                continue;
            }

            RequiredResources[type] = 0;
            ResourceStorage[type] = 0;
        }

        foreach (CraftingRecipeConfig config in AvailableCraftingRecipes) {
            RecipesToCraft[config.RecipeUid] = 0;
        }
    }
}