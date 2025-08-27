using UnityEngine;

public class InfoPanelView : MonoBehaviour {
    [SerializeField]
    private MainInfoPart _mainInfoPart;

    [SerializeField]
    private StorageInfoPart _storageInfoPart;

    [SerializeField]
    private CraftingInfoPart _craftingInfoPart;

    public void SetData(InfoDataCombined data) {
        _mainInfoPart.SetData(data.MainInfoData);

        if (data.StorageInfoData != null) {
            _storageInfoPart.SetData(data.StorageInfoData);
        } else {
            _storageInfoPart.Disable();
        }

        if (data.CraftingStation != null) {
            _craftingInfoPart.SetData(data.CraftingStation);
        } else {
            _craftingInfoPart.Disable();
        }
    }
}