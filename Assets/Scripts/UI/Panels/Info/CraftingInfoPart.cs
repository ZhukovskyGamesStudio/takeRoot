using System;
using System.Collections.Generic;
using CodeBase.Services;
using Settlers.Crafting;
using UnityEngine;

public class CraftingInfoPart : MonoBehaviour {
    [SerializeField]
    private CraftingLineView _craftingLineViewPrefab;

    [SerializeField]
    private List<CraftingRecipeConfig> _recipeConfigs;

    [SerializeField]
    private Transform _linesContainer;

    private CraftingStationData _craftingData;
    private IResearchService _researchService;

    public void SetData(CraftingStationData craftingStation) {
        _researchService = ServiceLocator.Container.Single<IResearchService>();
        gameObject.SetActive(true);
        _craftingData = craftingStation;

        foreach (Transform child in _linesContainer) {
            Destroy(child.gameObject);
        }

        foreach (var recipe in craftingStation.AvailableCraftingRecipes) {
            var line = Instantiate(_craftingLineViewPrefab, _linesContainer);
            if (recipe.RequiredResearch == Research.None || _researchService.GetInitResearchData()[recipe.RequiredResearch].Researchable)
                line.Set(recipe, _craftingData, AddRecipe, RemoveRecipe);
            else  
                line.Set(recipe, _craftingData);
        }
    }

    private void AddRecipe(CraftingRecipeConfig recipe) {
        _craftingData.AddRecipe(recipe.ResultingResource.ResourceType);
    }

    private void RemoveRecipe(CraftingRecipeConfig recipe) {
        _craftingData.RemoveRecipe(recipe.ResultingResource.ResourceType);
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}