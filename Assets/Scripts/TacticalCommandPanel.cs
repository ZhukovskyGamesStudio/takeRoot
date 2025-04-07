using System;
using UnityEngine;

public class TacticalCommandPanel : MonoBehaviour {
    public TacticalCommand SelectedTacticalCommand { get; private set; }

    public void SelectTacticalCommand(TacticalCommand tacticalCommand) {
        SelectedTacticalCommand = tacticalCommand;
        if (tacticalCommand == TacticalCommand.Move)
            CursorManager.ChangeCursor(CursorType.TacticalMove);
        if (tacticalCommand == TacticalCommand.TacticalAttack)
            CursorManager.ChangeCursor(CursorType.TacticalAttack);
    }

    public void ClearSelectedTacticalCommand(TacticalCommand tacticalCommand) {
        if (SelectedTacticalCommand == tacticalCommand) {
            SelectedTacticalCommand = TacticalCommand.None;
            CursorManager.ChangeCursor(CursorType.Normal);
        }
    }
}

[Serializable]
public enum TacticalCommand {
    None,
    Move,
    TacticalAttack,
    Cancel,
    Equip,
    Merge,
    RoundAttack,
    AddShootTarget
}