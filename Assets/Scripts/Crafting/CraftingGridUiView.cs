using System.Collections.Generic;
using System.Linq;
using Settlers.Crafting;
using UnityEngine;

public class CraftingGridUiView : MonoBehaviour {
    [SerializeField]
    private ResourceGridView _storage;

    [SerializeField]
    private CraftingLineUiView _craftingLineUiViewPrefab;

    private CraftingStationable _craftingStationable;

    private Dictionary<string, CraftingLineUiView> _craftingLineUiViews = new();

    public void Init(CraftingStationable craftingStationable) {
        foreach (CraftingLineUiView uiView in _craftingLineUiViews.Values) {
            Destroy(uiView.gameObject);
        }

        _craftingLineUiViews.Clear();

        _craftingStationable = craftingStationable;
        _craftingStationable.OnRecipeDataChanged += UpdateCraftingLineUiView;
        _craftingStationable.OnResourceStorageDataChanged += UpdateResourceStorageView;
        foreach (CraftingRecipeConfig recipe in craftingStationable.CraftingStationableData.AvailableRecipes) {
            CraftingLineUiView craftingLineView = Instantiate(_craftingLineUiViewPrefab, transform.position, Quaternion.identity, transform);

            _craftingLineUiViews.Add(recipe.RecipeUid, craftingLineView);

            craftingLineView.Set(recipe, this);
            UpdateCraftingLineUiView(recipe.RecipeUid);
        }
    }

    private void UpdateResourceStorageView() {
        _storage.FillGrid(_craftingStationable.GetStorageResourcesAsResourceDataList());
    }

    private void UpdateCraftingLineUiView(string recipeUid) {
        CraftingLineUiView craftingLineView = _craftingLineUiViews[recipeUid];
        int recipesToCraftCount = _craftingStationable.RecipesToCraftList.Count(r => r == recipeUid);
        craftingLineView.UpdateRecipesAmount(recipesToCraftCount);
        craftingLineView.UpdateRecipesAmountButtons(recipesToCraftCount);

        CraftingRecipeConfig recipe = ObsoleteCoreEntryPoint.CraftingManager.GetRecipe(recipeUid);
        foreach (ResourceData resource in recipe.RequiredResources) {
            ResourceData allAvailableResources = ResourceManager.FindAllAvailableResources(resource.ResourceType);
            int requiredResources = resource.Amount * recipesToCraftCount;

            craftingLineView.UpdateAmount(resource.ResourceType, requiredResources, allAvailableResources.Amount);
        }
    }

    public void ChangeRecipeToCraftAmount(string uid, int amount) {
        if (amount > 0) {
            _craftingStationable.AddRecipeToCraft(uid);
        } else if (amount < 0) {
            _craftingStationable.RemoveRecipeToCraft(uid);
        }

        UpdateCraftingLineUiView(uid);
    }
}