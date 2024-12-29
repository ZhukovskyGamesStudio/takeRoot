using UnityEngine;

public class CommandToggle : MonoBehaviour {
    [SerializeField]
    private Command _command;

    [SerializeField]
    private CommandsPanel _commandsPanel;

    public void OnValueChanged(bool val) {
        if (val) {
            _commandsPanel.SelectCommand(_command);
        } else {
            _commandsPanel.ClearSelectedCommand(_command);
        }
    }
}