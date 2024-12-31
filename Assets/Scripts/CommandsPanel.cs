using System;
using UnityEngine;

public class CommandsPanel : MonoBehaviour {
    public Command SelectedCommand { get; private set; }

    public void SelectCommand(Command command) {
        SelectedCommand = command;
        if (command == Command.Search) {
            CursorManager.ChangeCursor(CursorType.Search);
        }else if (command == Command.Attack) {
            CursorManager.ChangeCursor(CursorType.Wakeup);
        }
    }

    public void ClearSelectedCommand(Command command) {
        if (SelectedCommand == command) {
            SelectedCommand = Command.None;
            CursorManager.ChangeCursor(CursorType.Normal);
        }
    }
}

[Serializable]
public enum Command {
    None,
    Search,
    Attack,
    Cancel,
    Build
}