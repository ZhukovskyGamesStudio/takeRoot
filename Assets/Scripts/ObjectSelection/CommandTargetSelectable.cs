using UnityEngine;

public class CommandTargetSelectable : Selectable {
    [SerializeField]
    private CommandTarget _commandTarget;

    [SerializeField]
    private CraftingStation _craftingStation;

    public override object GetData() {
        var res = new InfoDataCombined() {
            MainInfoData = _commandTarget.Data.MainInfoData
        };
        if (_craftingStation != null) {
            res.CraftingStation = _craftingStation.stationData;
        }

        return res;
    }
}