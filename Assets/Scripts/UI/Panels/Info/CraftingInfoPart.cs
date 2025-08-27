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

    private CraftingStation _craftingStation;
    
    public void SetData(CraftingStation craftingStation) {
        gameObject.SetActive(true);
        _craftingStation = craftingStation;

        foreach (Transform child in _linesContainer) {
            Destroy(child.gameObject);
        }

        foreach (var recipe in craftingStation.AvailableCraftingRecipes) {
            var line = Instantiate(_craftingLineViewPrefab, _linesContainer);
            line.Set(recipe);
        }
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}