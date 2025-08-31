using System.Collections.Generic;
using Settlers.Crafting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingLineView : MonoBehaviour {
    private string _uid;

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

    public void Set(CraftingRecipeConfig config) {
        _config = config;
        _uid = config.RecipeUid;
        _explainText.text = config.MainInfo.Description;
        _result.SetData(config.MainInfo.Icon, config.MainInfo.Name);

        UpdateIngridients(config);
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

    public void ChangeRecipeToCraftAmount(int amount) {
        //_craftingGridUiView.ChangeRecipeToCraftAmount(_uid, amount);
    }

    public void UpdateAmount(ResourceType type, int queueAmount, int stockAmount) {
        //_requiredResourcesGridView.GetResourceView(type).SetAmount(queueAmount, $"{queueAmount} / {stockAmount}");
    }

    public void UpdateRecipesAmount(int recipesAmount) {
        _recipesToCraftAmount.text = $"{recipesAmount}";
    }

    public void UpdateRecipesAmountButtons(int recipesAmount) {
        _addButton.interactable = recipesAmount != 99;
        _removeButton.interactable = recipesAmount != 0;
    }
}