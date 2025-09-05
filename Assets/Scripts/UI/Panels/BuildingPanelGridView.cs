using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanelGridView : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _name, _selectedNameText;

    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Toggle _toggle;

    private string _id;
    
    [SerializeField]

    private BuildingRecipeConfig _config;
    private Action<BuildingRecipeConfig> _openPanel;

    public void Init(Action<BuildingRecipeConfig> openPanel, ToggleGroup toggleGroup) {
        _openPanel = openPanel;
        _toggle.group = toggleGroup;
    }

    public void SetData(BuildingRecipeConfig config) {
        _config = config;
        _name.text = config.mainInfo.Name;
        _selectedNameText.text = config.mainInfo.Name;
        _icon.sprite = _config.mainInfo.Icon;
        _toggle.isOn = false;
    }

    public void Open(bool isOn) {
        if (!isOn) {
            return;
        }

        _openPanel?.Invoke(_config);
    }
}