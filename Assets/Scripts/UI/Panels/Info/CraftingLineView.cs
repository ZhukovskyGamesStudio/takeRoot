using System;
using System.Collections.Generic;
using Settlers.Crafting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingLineView : MonoBehaviour {
   [SerializeField]
    private ImageTextPair _result;

    [SerializeField]
    private TextMeshProUGUI _explainText;

    [SerializeField]
    private Button _addButton, _removeButton;

    [SerializeField]
    private List<ImageTextPair> _ingridients;

    [SerializeField]
    private TextMeshProUGUI _recipesToCraftAmount;

    [SerializeField]
    private TextMeshProUGUI _amountText;

    [SerializeField]
    private ResourcesTable _resourcesTable;

    private CraftingRecipeConfig _config;
    private Action<CraftingRecipeConfig> _addRecipe, _removeRecipe;
    private CraftingStationData _data;

    public void Set(CraftingRecipeConfig config, CraftingStationData data, Action<CraftingRecipeConfig> addRecipe,
        Action<CraftingRecipeConfig> removeRecipe) {
        _config = config;
        _data = data;
        _addRecipe = addRecipe;
        _removeRecipe = removeRecipe;
        _explainText.text = config.MainInfo.Description;
        _result.SetData(config.MainInfo.Icon, config.MainInfo.Name);

        UpdateIngridients(config);
        
        UpdateRecipesAmount(GetQueuedAmount);
        UpdateRecipesAmountButtons(GetQueuedAmount);
    }

    public void Set(CraftingRecipeConfig config, CraftingStationData data) {
        _config = config;
        _data = data;
        _explainText.text = config.MainInfo.Description;
        _result.SetDataUnavailable(config.MainInfo.Icon, config.MainInfo.Name);

        foreach (var ingridient in _ingridients) {
            ingridient.gameObject.SetActive(false);
        }
        _addButton.gameObject.SetActive(false);
        _removeButton.gameObject.SetActive(false);
        _amountText.transform.parent.gameObject.SetActive(false);
    }

    private void UpdateIngridients(CraftingRecipeConfig config) {
        foreach (ImageTextPair ingridient in _ingridients) {
            ingridient.gameObject.SetActive(false);
        }

        for (int index = 0; index < config.RequiredResources.Count; index++) {
            ResourceData res = config.RequiredResources[index];
            _ingridients[index].SetData(_resourcesTable.ResourceIconsDictionary[res.ResourceType], $"{0}/{res.Amount}");
            _ingridients[index].gameObject.SetActive(true);
        }
    }

    private void Update() {
        int queuedAmount = GetQueuedAmount;
        UpdateRecipesAmount(queuedAmount);
        UpdateRecipesAmountButtons(queuedAmount);
    }

    public void Add() {
        _addRecipe?.Invoke(_config);
    }

    public void Remove() {
        _removeRecipe?.Invoke(_config);
    }

    private int GetQueuedAmount => _data.RecipesToCraft[_config.ResultingResource.ResourceType];

    public void UpdateRecipesAmount(int recipesAmount) {
        _recipesToCraftAmount.text = $"{recipesAmount}";
    }

    public void UpdateRecipesAmountButtons(int recipesAmount) {
        _addButton.interactable = recipesAmount != 99;
        _removeButton.interactable = recipesAmount != 0;
    }
}