using System;
using UnityEngine;
using UnityEngine.UI;

public class CommandToggle : MonoBehaviour {
    [SerializeField]
    private Command _command;

    [SerializeField]
    private CommandsPanel _commandsPanel;

    [SerializeField]
    private Toggle _toggle;

    [SerializeField]
    private KeyCode _keyCode;

    private void Update() {
        if (Input.GetKeyDown(_keyCode)) {
            _toggle.isOn = !_toggle.isOn;
        }
    }

    private void OnDisable() {
        _toggle.SetIsOnWithoutNotify(false);
    }

    public void OnValueChanged(bool val) {
        if (val) {
            _commandsPanel.SelectCommand(_command);
        } else {
            _commandsPanel.ClearSelectedCommand(_command);
        }
    }
}