using UnityEngine;
using UnityEngine.UI;

public class TacticalCommandToggle : MonoBehaviour {
    [SerializeField]
    private TacticalCommand _tacticalCommand;

    [SerializeField]
    private TacticalCommandPanel _tacticalCommandPanel;

    [SerializeField]
    private Toggle _toggle;

    [SerializeField]
    private KeyCode _keyCode;

    private void Update() {
        if (Input.GetKeyDown(_keyCode)) {
            _toggle.isOn = !_toggle.isOn;
        }
    }

    public void OnValueChanged(bool val) {
        if (val) {
            _tacticalCommandPanel.SelectTacticalCommand(_tacticalCommand);
        } else {
            _tacticalCommandPanel.ClearSelectedTacticalCommand(_tacticalCommand);
        }
    }
}