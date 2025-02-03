using System;
using UnityEngine;
using UnityEngine.UI;

public class ChangeModeToggle : MonoBehaviour {
    [SerializeField]
    private CommandsPanel _commandsPanel;

    [SerializeField]
    private TacticalCommandPanel _tacticalCommandPanel;

    [SerializeField]
    private Toggle _toggle;

    [SerializeField]
    private KeyCode _hotkey;

    private void Update() {
        if (Input.GetKeyDown(_hotkey)) {
            _toggle.isOn = !_toggle.isOn;
        }
    }

    public void SetToggleValue(bool isOn) {
        _toggle.SetIsOnWithoutNotify(isOn);
    }

    public void OnValueChanged(bool val) {
        SettlersSelectionManager.Instance.TryChangeSelectedSettlerMode(val);
        ChangeCommandsPanelsWithSettlerMode();
    }

    private void ChangeCommandsPanelsWithSettlerMode() {
        var settler = SettlersSelectionManager.Instance.SelectedSettler;
        if (settler != null) {
            switch (settler.Mode) {
                case Mode.Planning:
                    ChangePanels(false);
                    break;
                case Mode.Tactical:
                    ChangePanels(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(settler.Mode), settler.Mode, null);
            }
        }
    }

    private void ChangePanels(bool isTactical) {
        CommandsManagersHolder.Instance.TacticalCommandsManager.SetActivePanel(isTactical);
        CommandsManagersHolder.Instance.CommandsManager.SetActivePanel(!isTactical);
    }
}