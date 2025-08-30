using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsPanelInfoPage : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _header, _description, _footprint, _hp;

    [SerializeField]
    private List<Image> _ingridientImages;

    [SerializeField]
    private List<TextMeshProUGUI> _ingridientCountTexts;

    [SerializeField]
    private ResourcesTable _resourcesTable;

    [SerializeField]
    private GameObject _notSelectedState, _selectedState;

    public void SetEmptyData() {
        _notSelectedState.gameObject.SetActive(true);
        _selectedState.gameObject.SetActive(false);
    }

    public void SetData(BuildingRecipeConfig recipeConfig) {
        _notSelectedState.gameObject.SetActive(false);
        _selectedState.gameObject.SetActive(true);
        _header.text = recipeConfig.mainInfo.Name;
        _description.text = recipeConfig.mainInfo.Description;
        _footprint.text = $"{recipeConfig.Footprint.x}x{recipeConfig.Footprint.y}";
        _hp.text = $"{recipeConfig.Hp}";

        foreach (Image ingridient in _ingridientImages) {
            ingridient.gameObject.SetActive(false);
        }

        int shownIngridients = Mathf.Min(_ingridientImages.Count, recipeConfig.Ingridients.Count);
        for (int i = 0; i < shownIngridients; i++) {
            _ingridientImages[i].gameObject.SetActive(true);
            ResourceType type = recipeConfig.Ingridients.Keys.ElementAt(i);
            _ingridientImages[i].sprite = _resourcesTable.ResourceIconsDictionary[type];
            _ingridientCountTexts[i].text = recipeConfig.Ingridients[type].ToString();
        }
    }
}