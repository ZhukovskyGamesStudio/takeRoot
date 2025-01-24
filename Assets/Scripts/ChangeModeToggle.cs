using UnityEngine;
using UnityEngine.UI;

public class ChangeModeToggle : MonoBehaviour {
    [SerializeField]
    private CommandsPanel _commandsPanel;

    [SerializeField]
    private TacticalCommandPanel _tacticalCommandPanel;

    [SerializeField]
    private Toggle _toggle;

    public void SetToggleValue(bool isOn) {
        _toggle.SetIsOnWithoutNotify(isOn);
    }

    public void OnValueChanged(bool val) {
        SettlersSelectionManager.Instance.TryChangeSelectedSettlerMode(val);
    }
}