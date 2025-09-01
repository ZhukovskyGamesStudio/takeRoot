using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using Settlers.Crafting;
using UnityEngine;

public class CraftingStation : MonoBehaviour {
    [field: SerializeField]
    public CraftingStationData StationData { get; private set; }

    [SerializeField]
    private Progress _progressData;
    
    [HideInInspector]
    public Dictionary<ResourceType, int> ReservedRequiredResources;

    [HideInInspector]
    public AYellowpaper.SerializedCollections.SerializedDictionary<Race, AI.Settler> Crafters = new();

    public Vector3? HaulInteractPos => _gridObject.GetNeighborFreeTile();
    //[HideInInspector]
    public List<Transform> InteractPos = new List<Transform>(2);

    private ICraftingService _craftingService;
    private IResourceManager _resourceManager;
    private GridObject _gridObject;

    private void Start() {
        _resourceManager = ServiceLocator.Container.Single<IResourceManager>();
        _craftingService = ServiceLocator.Container.Single<ICraftingService>();
        _gridObject = GetComponent<GridObject>();
        _gridObject.UpdatePosition();
        _craftingService.AddCraftingStation(this);
        Crafters = new() {
            { Race.Plants, null},
            { Race.Robots, null}
        };
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

    public bool CanCraft() {
        PickNewRecipe();
        var canCraft = false;
        foreach (CraftingRecipeConfig config in StationData.AvailableCraftingRecipes) {
            if (StationData.RecipesToCraft[config.ResultingResource.ResourceType] == 0) continue;
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
        _progressData.ProgressData.Progress = StationData.CurrentRecipeCraftingPoints;
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

        _resourceManager.SpawnResource(InteractPos[0].position, StationData.CurrentRecipe.ResultingResource.ResourceType, StationData.CurrentRecipe.ResultingResource.Amount);
        StationData.CurrentRecipeCraftingPoints = 0;
        StationData.CurrentRecipe = null;
        _progressData.ProgressData.InfoViewEnabled = false;
        _progressData.ProgressData.Progress = 0;
        Debug.Log($"Crafted {resource.ResourceType}");
    }

    private void PickNewRecipe() {
        CraftingRecipeConfig recipe = null;
        foreach (CraftingRecipeConfig config in StationData.AvailableCraftingRecipes) {
            if (StationData.RecipesToCraft[config.ResultingResource.ResourceType] == 0) continue;
            foreach (ResourceData resource in config.RequiredResources) {
                if (StationData.ResourceStorage[resource.ResourceType] < resource.Amount) {
                    break;
                }

                StationData.CurrentRecipe = StationData.AvailableCraftingRecipes.FirstOrDefault(r => r.ResultingResource.ResourceType == config.ResultingResource.ResourceType);
                
                _progressData.ProgressData.InfoViewEnabled = true;
                _progressData.ProgressData.Needed = StationData.CurrentRecipe!.CraftingPoints;
                _progressData.ProgressData.Title = StationData.CurrentRecipe.MainInfo.Name;
                return;
            }
        }
    }
}