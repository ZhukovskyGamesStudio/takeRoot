using UnityEngine;

public class SettlerSelectable : Selectable {
    [SerializeField]
    private AI.Settler _settler;

    public override object GetData() {
        return _settler.Data;
    }
}