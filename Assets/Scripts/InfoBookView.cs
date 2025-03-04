using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoBookView : MonoBehaviour {
    public static bool IsAutoOpenInfoPanel;

    [SerializeField]
    private Image _icon;

    [SerializeField]
    private TextMeshProUGUI _nameText;

    [SerializeField]
    private ResourceGridView _resourceGridView;

    [SerializeField]
    private EquipmentGridUiView _equipmentGridView;

    [SerializeField]
    private CraftingGridUiView _craftingGridUiView;

    [field: SerializeField]
    public Toggle _infoToggle;

    public void Init(InfoBookData data) {
        _icon.sprite = data.Icon;
        _nameText.text = data.Name;

        _resourceGridView.FillGrid(data.Resources);
        _equipmentGridView.gameObject.SetActive(false);

        if (data is CraftingStationInfoBookData) {
            _craftingGridUiView.gameObject.SetActive(true);
            _craftingGridUiView.Set();
            //(CraftingStationInfoBookData)data)....
        }
    }

    public void Init(SettlerData settlerData) {
        _icon.sprite = settlerData.InfoBookIcon;
        _nameText.text = settlerData.Name;

        _equipmentGridView.gameObject.SetActive(true);
        _equipmentGridView.Set(settlerData);
        _craftingGridUiView.gameObject.SetActive(false);
    }

    public void SetToggle(bool isOn) {
        _infoToggle.isOn = isOn;
    }

    public bool GetToggle() {
        return _infoToggle.isOn;
    }
}

public class InfoBookData {
    public Sprite Icon;
    public string Name;

    public List<ResourceData> Resources;
}

public class CraftingStationInfoBookData : InfoBookData {
    public List<string> CraftingReceipts;
}