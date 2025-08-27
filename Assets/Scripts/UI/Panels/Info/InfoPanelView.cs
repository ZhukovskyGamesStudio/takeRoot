using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelView : MonoBehaviour {
    [field: SerializeField]
    public bool IsAutoOpenInfoPanel { get; private set; }

    [SerializeField]
    private ImageTextPair _mainIconText;

    [SerializeField]
    private CraftingGridUiView _craftingGridUiView;

    [SerializeField]
    private StorageInfoPart _storageInfoPart;

    [SerializeField]
    private CraftingInfoPart _craftingInfo;

    [field: SerializeField]
    public Toggle _infoToggle;

    public void SetData(CommandTargetData data) {
        Init(data.InfoPanelData);
    }

    public void Init(InfoPanelData data) {
        _craftingGridUiView.gameObject.SetActive(false);

        _mainIconText.SetData(data.Icon, data.Name);
        _storageInfoPart.SetData(data.Resources);
    }

    public void Init(CraftingStationable craftingStationable) {
        _storageInfoPart.Disable();
        _mainIconText.SetData(craftingStationable.CraftingStationableData.InfoBookIcon, craftingStationable.CraftingStationableData.Name);
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
public class InfoPanelData {
    public Sprite Icon;
    public string Name;

    public List<ResourceData> Resources;
}