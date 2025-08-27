using UnityEngine;

public class CommandTargetSelectable : Selectable {
    [SerializeField]
    private CommandTarget _commandTarget;

    [SerializeField]
    private Storage _storage;

    [SerializeField]
    private CraftingStation _craftingStation;
    
    //todo cache all components in awake

    public override object GetData() {
        var res = new InfoDataCombined() {
            MainInfoData = _commandTarget.Data.MainInfoData
        };

        if (_storage != null) {
            res.StorageData = _storage.StorageData;
        }

        if (_craftingStation != null) {
            res.CraftingStation = _craftingStation.StationData;
        }

        return res;
    }
}