using UnityEngine;

public class SettlerSelectable : Selectable {
    [SerializeField]
    private AI.Settler _settler;

    [SerializeField]
    private MainInfoData _mainInfoData;

    
    public AI.Settler Settler => _settler;

    public override object GetData(Race race) {
        if (race == _settler.Data.names.Race) {
            return _settler.Data;
        }
        var res = new InfoDataCombined {
            MainInfoData = _mainInfoData
        };
        res.MainInfoData.Name = _settler.Data.names.Name;

        return res;
    }
}