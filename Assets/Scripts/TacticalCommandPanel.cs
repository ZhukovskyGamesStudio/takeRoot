

using System;
using UnityEngine;

public class TacticalCommandPanel : MonoBehaviour
{
    public TacticalCommand SelectedTacticalCommand { get; private set; }

    private void Update()
    {
       //Debug.Log(SelectedTacticalCommand);
    }

    public void SelectTacticalCommand(TacticalCommand tacticalCommand)
    {
        SelectedTacticalCommand = tacticalCommand;
        //TODO: Change Cursor
    }

    public void ClearSelectedTacticalCommand(TacticalCommand tacticalCommand)
    {
        if (SelectedTacticalCommand == tacticalCommand)
        {
            SelectedTacticalCommand = TacticalCommand.None;
            CursorManager.ChangeCursor(CursorType.Normal);
        }
    }
    
}

[Serializable]
public enum TacticalCommand
{
    None,
    Move,
    TacticalAttack,
    Cancel
}