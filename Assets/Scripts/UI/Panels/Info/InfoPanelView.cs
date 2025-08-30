using UnityEngine;

public class InfoPanelView : MonoBehaviour {
    [SerializeField]
    private MainInfoPart _mainInfoPart;

    [SerializeField]
    private StorageInfoPart _storageInfoPart;

    [SerializeField]
    private CraftingInfoPart _craftingInfoPart;
    
    [SerializeField]
    private ProgressInfoPart _progressInfoPart;

    public void SetData(InfoDataCombined data) {
        _mainInfoPart.SetData(data.MainInfoData);

        if (data.StorageData != null) {
            _storageInfoPart.SetData(data.StorageData);
        } else {
            _storageInfoPart.Disable();
        }

        if (data.CraftingStation != null) {
            _craftingInfoPart.SetData(data.CraftingStation);
        } else {
            _craftingInfoPart.Disable();
        }

        if (data.ProgressData != null) {
            _progressInfoPart.SetData(data.ProgressData);
        } else {
            _progressInfoPart.Disable();
        }
    }
}