using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using Settlers.Crafting;
using Unity.Netcode;
using UnityEngine;

public class CraftingStation : NetworkBehaviour {
    [field: SerializeField]
    public CraftingStationData StationData { get; private set; }

    [SerializeField]
    private Progress _progressData;

    [SerializeField]
    private Storage _storage;

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

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        _resourceManager = ServiceLocator.Container.Single<IResourceManager>();
        _craftingService = ServiceLocator.Container.Single<ICraftingService>();
        _gridObject = GetComponent<GridObject>();
        _gridObject.UpdatePosition();
        _craftingService.AddCraftingStation(this);
        Crafters = new() {
            { Race.Plants, null },
            { Race.Robots, null }
        };
        ReservedRequiredResources = new Dictionary<ResourceType, int>();
        foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
            if (type == ResourceType.None) {
                continue;
            }

            ReservedRequiredResources[type] = 0;
        }

        StationData.Init(this);
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

        SetResourceClientRpc(type, StationData.ResourceStorage[type]);
    }

    [ClientRpc]
    private void SetResourceClientRpc(ResourceType type, int amount) {
        StationData.ResourceStorage[type] = amount;
        var r = _storage.StorageData.Resources.FirstOrDefault(rr => rr.ResourceType == type);
        if (amount == 0 && r != null) {
            r.Amount = 0;
            r.ResourceType = ResourceType.None;
        }

        if (r != null) {
            r.Amount = amount;
        } else {
            var emptySpace = _storage.StorageData.Resources.FirstOrDefault(rrrr => rrrr.ResourceType == ResourceType.None);
            emptySpace.ResourceType = type;
            emptySpace.Amount = amount;
        }
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
        UpdateCraftingPoints();

        if (StationData.CurrentRecipe.CraftingPoints == StationData.CurrentRecipeCraftingPoints) {
            CraftResource();
            PickNewRecipe();
            SetProgressDataClientRpc(_progressData.ProgressData.InfoViewEnabled, _progressData.ProgressData.Needed,
                _progressData.ProgressData.Title);
        }

        SyncCraftingPointsClientRpc(StationData.CurrentRecipeCraftingPoints);
    }

    private void UpdateCraftingPoints() {
        _progressData.ProgressData.Progress.Value = StationData.CurrentRecipeCraftingPoints;
    }

    [ClientRpc]
    private void SyncCraftingPointsClientRpc(int points) {
        StationData.CurrentRecipeCraftingPoints = points;
        _progressData.ProgressData.Progress.Value = points;
    }

    private void CraftResource() {
        var resource = StationData.CurrentRecipe.ResultingResource;
        foreach (ResourceData requiredResources in StationData.CurrentRecipe.RequiredResources) {
            StationData.ResourceStorage[requiredResources.ResourceType] -= requiredResources.Amount;
            StationData.RequiredResources[requiredResources.ResourceType] -= requiredResources.Amount;
            SetResourceClientRpc(requiredResources.ResourceType, StationData.ResourceStorage[requiredResources.ResourceType]);
        }

        _resourceManager.SpawnResource(InteractPos[0].position, StationData.CurrentRecipe.ResultingResource.ResourceType,
            StationData.CurrentRecipe.ResultingResource.Amount);
        StationData.CurrentRecipeCraftingPoints = 0;
        StationData.CurrentRecipe = null;
        _progressData.ProgressData.InfoViewEnabled = false;
        _progressData.ProgressData.Progress.Value = 0;
        SyncCraftingPointsClientRpc(StationData.CurrentRecipeCraftingPoints);

        Debug.Log($"Crafted {resource.ResourceType}");
    }

    private void PickNewRecipe() {
        foreach (CraftingRecipeConfig config in StationData.AvailableCraftingRecipes) {
            if (StationData.RecipesToCraft[config.ResultingResource.ResourceType] == 0) continue;
            foreach (ResourceData resource in config.RequiredResources) {
                if (StationData.ResourceStorage[resource.ResourceType] < resource.Amount) {
                    break;
                }

                var nextRecipe =
                    StationData.AvailableCraftingRecipes.FirstOrDefault(r =>
                        r.ResultingResource.ResourceType == config.ResultingResource.ResourceType);

                StationData.CurrentRecipe = nextRecipe;

                SetProgressData();

                return;
            }
        }
    }

    private void SetProgressData() {
        _progressData.ProgressData.InfoViewEnabled = true;
        _progressData.ProgressData.Needed = StationData.CurrentRecipe!.CraftingPoints;
        _progressData.ProgressData.Title = StationData.CurrentRecipe.MainInfo.Name;
    }

    [ClientRpc]
    private void SetProgressDataClientRpc(bool info, int needed, string title) {
        _progressData.ProgressData.InfoViewEnabled = info;
        _progressData.ProgressData.Needed = needed;
        _progressData.ProgressData.Title = title;
    }

    public void AddRecipe(ResourceType recipeRes) {
        AddRecipeServerRpc(recipeRes);
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddRecipeServerRpc(ResourceType recipeRes) {
        StationData.AddRecipe(recipeRes);
        SetRecipeAmountClientRpc(recipeRes, StationData.RecipesToCraft[recipeRes]);
    }

    public void RemoveRecipe(ResourceType recipeRes) {
        RemoveRecipeServerRpc(recipeRes);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RemoveRecipeServerRpc(ResourceType recipeRes) {
        StationData.RemoveRecipe(recipeRes);
        SetRecipeAmountClientRpc(recipeRes, StationData.RecipesToCraft[recipeRes]);
    }

    [ClientRpc]
    private void SetRecipeAmountClientRpc(ResourceType recipeRes, int amount) {
        StationData.RecipesToCraft[recipeRes] = amount;
    }
}