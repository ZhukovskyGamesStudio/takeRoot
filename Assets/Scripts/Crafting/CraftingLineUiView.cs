using System;
using System.Collections.Generic;
using Settlers.Crafting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingLineUiView : MonoBehaviour
{
    private string _uid;
    private CraftingGridUiView _craftingGridUiView;
    
    [SerializeField]
    private Image _icon;
    
    [SerializeField]
    private TextMeshProUGUI _headerText, _explainText;
    
    [SerializeField]
    private Button _addButton, _removeButton;
    
    [SerializeField]
    private TextMeshProUGUI _recipesToCraftAmount;
    
    [SerializeField]
    private TextMeshProUGUI _amountText;

    [SerializeField]
    private ResourceGridView _requiredResourcesGridView;
    
    public void Set(CraftingRecipeConfig config, CraftingGridUiView craftingGridUiView)
    {
        _uid = config.RecipeUid;
        _headerText.text = config.ResultingResource.ResourceType.ToString();
        _icon.sprite = config.RecipeIcon;
        _craftingGridUiView = craftingGridUiView;
        
        _requiredResourcesGridView.FillGrid(config.RequiredResources);
        
        //_explainText.text = config.ExplainText;
    }
    public void ChangeRecipeToCraftAmount(int amount)
    {
        _craftingGridUiView.ChangeRecipeToCraftAmount(_uid, amount);
    }
    
    public void UpdateAmount(ResourceType type, int queueAmount, int stockAmount) {
        _requiredResourcesGridView.GetResourceView(type).SetAmount(queueAmount, $"{queueAmount} / {stockAmount}");
    }

    public void UpdateRecipesAmount(int recipesAmount)
    {
        _recipesToCraftAmount.text = $"{recipesAmount}";
    }

    public void UpdateRecipesAmountButtons(int recipesAmount)
    {
        _addButton.interactable = recipesAmount != 99;
        _removeButton.interactable = recipesAmount != 0;
    }
}