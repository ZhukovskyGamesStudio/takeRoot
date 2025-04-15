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
    None = 0,
    Search = 1,
    Break = 2,
    Cancel = 3,
    Build = 4,
    Store = 5,
    Transport = 6,
    GatherResources = 7,
    Delivery = 8,
    GatherResourcesForCraft = 9,
    DeliveryForCraft = 10,
    Craft = 11,
    PrepareToCraft = 12,
    Water = 13
}