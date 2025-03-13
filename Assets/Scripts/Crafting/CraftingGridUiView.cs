using System;
using System.Collections.Generic;
using Settlers.Crafting;
using UnityEngine;
using UnityEngine.Serialization;

public class CraftingGridUiView : MonoBehaviour
{
    [SerializeField]private CraftingLineUiView _craftingLineUiViewPrefab;
    private CraftingStationable _craftingStationable;
    
    private Dictionary<string, CraftingLineUiView> _craftingLineUiViews = new Dictionary<string, CraftingLineUiView>();
    
    public void Init(CraftingStationable craftingStationable)
    {
        foreach (CraftingLineUiView uiView in _craftingLineUiViews.Values)
        {
            Destroy(uiView.gameObject);
        }
        _craftingLineUiViews.Clear();
        
        _craftingStationable = craftingStationable;
        foreach (CraftingRecipeConfig recipe in craftingStationable.CraftingStationableData.AvailableRecipes)
        {
            var craftingLineView = Instantiate(_craftingLineUiViewPrefab, transform.position, Quaternion.identity, transform);
            
            _craftingLineUiViews.Add(recipe.RecipeUid, craftingLineView);
            
            craftingLineView.Set(recipe, this);
            UpdateCraftingLineUiView(recipe.RecipeUid);
        }
    }

    private void UpdateCraftingLineUiView(string recipeUid)
    {
        var craftingLineView = _craftingLineUiViews[recipeUid];
        craftingLineView.UpdateRecipesAmount(_craftingStationable.RecipesToCraft[recipeUid]);
        craftingLineView.UpdateRecipesAmountButtons(_craftingStationable.RecipesToCraft[recipeUid]);
        
        var recipe = Core.CraftingManager.GetRecipe(recipeUid);
        foreach (ResourceData resource in recipe.RequiredResources)
        {
            var allAvailableResources = ResourceManager.FindAllAvailableResources(resource.ResourceType);
            var requiredResources = resource.Amount * _craftingStationable.RecipesToCraft[recipeUid];
            
            craftingLineView.UpdateAmount(resource.ResourceType, requiredResources, allAvailableResources.Amount);
        }        
    }

    public void ChangeRecipeToCraftAmount(string uid, int amount)
    {
        if (amount > 0)
            _craftingStationable.AddRecipeToCraft(uid);
        else if (amount < 0)
            _craftingStationable.RemoveRecipeToCraft(uid);
        
        UpdateCraftingLineUiView(uid);
    }
}