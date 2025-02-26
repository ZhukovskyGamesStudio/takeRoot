using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandsPanel : MonoBehaviour {
    [SerializeField]
    private List<CommandToggle> _commandToggles;

    public Command SelectedCommand { get; private set; }

    public void SelectCommand(Command command) {
        SelectedCommand = command;
        if (command == Command.Search) {
            CursorManager.ChangeCursor(CursorType.Search);
        } else if (command == Command.Break) {
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
    Break,
    Cancel,
    Build,
    Store,
    Transport,
    GatherResources,
    Delivery,
    Craft
}