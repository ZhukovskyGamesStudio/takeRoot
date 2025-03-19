using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Settlers.Crafting;
using UnityEngine;

public class CraftingStationable : ECSComponent
{
    public Action<string> OnRecipeDataChanged;
    public Action OnResourceStorageDataChanged;
    
    private CraftingCombinedCommand _craftingCombinedCommand;
    
    private Dictionary<string, int> _recipesToCraft = new Dictionary<string, int>();
    private Dictionary<ResourceType, int> _resourceStorage = new Dictionary<ResourceType, int>();
    private Dictionary<ResourceType, int> _requiredResourcesForAllCurrentCrafts = new Dictionary<ResourceType, int>();

    public Interactable Interactable { get; private set; }
    public CraftingRecipeConfig CurrentRecipeToCraft { get; private set; }
    public ReadOnlyDictionary<string, int> RecipesToCraft => new(_recipesToCraft);
    public ReadOnlyDictionary<ResourceType, int> ResourceStorage => new(_resourceStorage);
    public ReadOnlyDictionary<ResourceType, int> RequiredResourcesForAllCurrentCrafts => new(_requiredResourcesForAllCurrentCrafts);
    public CraftingStationableData CraftingStationableData { get; private set; }

    private void Update()
    {
        _craftingCombinedCommand.UpdateLogic();
    }

    public override void Init(ECSEntity entity) {
        Interactable = entity.GetEcsComponent<Interactable>();
        CraftingStationableData = entity.GetEcsComponent<CraftingStationableData>();
        
        foreach (CraftingRecipeConfig recipe in CraftingStationableData.AvailableRecipes)
        {
            _recipesToCraft.Add(recipe.RecipeUid, 0);
        }
        foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
        {
            if (type == ResourceType.None) continue;
            _resourceStorage.Add(type, 0);
            _requiredResourcesForAllCurrentCrafts.Add(type, 0);
        }
        
        _craftingCombinedCommand = new CraftingCombinedCommand(this);
    }

    public int GetResourceAmountFromStorage(ResourceType type)
    {
        return _resourceStorage[type];
    }
    
    public ResourceData GetResourceDataFromStorage(ResourceType type)
    {
        return new ResourceData()
        {
            ResourceType = type,
            Amount = ResourceStorage[type]
        };
    }
    
    public void AddResourceToStorage(ResourceData data)
    {
        _resourceStorage[data.ResourceType] += data.Amount;
        _craftingCombinedCommand.RemoveReservation(data);
        OnResourceStorageDataChanged?.Invoke();
    }
    
    public void RemoveResourceFromStorage(ResourceData data)
    {
        if (ResourceStorage[data.ResourceType] < data.Amount) return;
        _resourceStorage[data.ResourceType] -= data.Amount;
        OnResourceStorageDataChanged?.Invoke();
    }
    
    public void AddRecipeToCraft(string uid)
    {
        var recipe = Core.CraftingManager.GetRecipe(uid);
        if (RecipesToCraft[uid] == 99)
            return;
        _recipesToCraft[uid]++;
        foreach (ResourceData resource in recipe.RequiredResources)
        {
            _requiredResourcesForAllCurrentCrafts[resource.ResourceType] += resource.Amount;
        }
        OnRecipeDataChanged?.Invoke(uid);
    }
    
    public void RemoveRecipeToCraft(string uid)
    {
        var recipe = Core.CraftingManager.GetRecipe(uid);
        if (RecipesToCraft[uid] == 0)
            return;
        _recipesToCraft[uid]--;
        foreach (ResourceData resource in recipe.RequiredResources)
        {
            _requiredResourcesForAllCurrentCrafts[resource.ResourceType] -= resource.Amount;
        }
        OnRecipeDataChanged?.Invoke(uid);
    }
    
    public void SetCurrentCraft(CraftingRecipeConfig recipe)
    {
        CurrentRecipeToCraft = recipe;
    }
    
    public void CancelCurrentCraft()
    {
        CurrentRecipeToCraft = null;
        _craftingCombinedCommand.CancelCraftingCommandsRightAway();
    }
    
    public void CraftItem()
    {
        foreach (ResourceData resource in CurrentRecipeToCraft.RequiredResources)
        {
            RemoveResourceFromStorage(resource);
        }
        var resultResource = CurrentRecipeToCraft.ResultingResource;
        ResourceManager.SpawnResourcesAround(new List<ResourceData>(){resultResource}, VectorUtils.ToVector2Int(Interactable.Gridable.GetCenterOnGrid)); //TODO: SpawnResAround overload with occupiedCells
        RemoveRecipeToCraft(CurrentRecipeToCraft.RecipeUid);
        CurrentRecipeToCraft = null;
    }

    public List<ResourceData> GetStorageResourcesAsResourceDataList()
    {
        List<ResourceData> resources = new List<ResourceData>(10);
        foreach (KeyValuePair<ResourceType,int> resource in _resourceStorage)
        {
            if (resource.Value == 0) continue;
            resources.Add(new ResourceData(){ResourceType = resource.Key, Amount = resource.Value});
        }
        return resources;
    }
    public override int GetDependancyPriority() {
        return 3;
    }
}