using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoBookView : MonoBehaviour {
    [field: SerializeField]
    public bool IsAutoOpenInfoPanel { get; private set; }

    [SerializeField]
    private Image _icon;

    [SerializeField]
    private TextMeshProUGUI _nameText;

    [SerializeField]
    private ResourceGridView _resourceGridView;

    [SerializeField]
    private CraftingGridUiView _craftingGridUiView;

    [SerializeField]
    private EquipmentGridUiView _equipmentGridView;

    [field: SerializeField]
    public Toggle _infoToggle;

    public void SetData(CommandTargetData data) {
        Init(data.InfoBookData);
    }

    public void Init(InfoBookData data) {
        _craftingGridUiView.gameObject.SetActive(false);

        _icon.sprite = data.Icon;
        _nameText.text = data.Name;

        _resourceGridView.FillGrid(data.Resources);
        _equipmentGridView.gameObject.SetActive(false);
    }

    public void Init(CraftingStationable craftingStationable) {
        _equipmentGridView.gameObject.SetActive(false);
        _resourceGridView.gameObject.SetActive(false);

        _icon.sprite = craftingStationable.CraftingStationableData.InfoBookIcon;
        _nameText.text = craftingStationable.CraftingStationableData.Name;
        _craftingGridUiView.Init(craftingStationable);
        _craftingGridUiView.gameObject.SetActive(true);
    }

    public void SetToggle(bool isOn) {
        _infoToggle.isOn = isOn;
    }

    public bool GetToggle() {
        return _infoToggle.isOn;
    }
}

[Serializable]
public class InfoBookData {
    public Sprite Icon;
    public string Name;

    public List<ResourceData> Resources;
}