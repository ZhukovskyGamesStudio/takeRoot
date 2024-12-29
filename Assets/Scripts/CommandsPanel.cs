using System;
using UnityEngine;

public class CommandsPanel : MonoBehaviour {
    public Command SelectedCommand { get; private set; }

    public void SelectCommand(Command command) {
        SelectedCommand = command;
    }

    public void ClearSelectedCommand(Command command) {
        if (SelectedCommand == command) {
            SelectedCommand = Command.None;
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