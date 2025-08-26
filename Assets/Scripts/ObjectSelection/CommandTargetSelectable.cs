using UnityEngine;

public class CommandTargetSelectable : Selectable {
    [SerializeField]
    private CommandTarget _commandTarget;

    public override object GetData() {
        return _commandTarget.Data;
    }
}