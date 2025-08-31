using System;
using System.Collections.Generic;
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

    public void SetData(CraftingStationData craftingStation) {
        gameObject.SetActive(true);
        _craftingData = craftingStation;

        foreach (Transform child in _linesContainer) {
            Destroy(child.gameObject);
        }

        foreach (var recipe in craftingStation.AvailableCraftingRecipes) {
            var line = Instantiate(_craftingLineViewPrefab, _linesContainer);
            line.Set(recipe, _craftingData, AddRecipe, RemoveRecipe);
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