using System.Collections;
using System.Linq;
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

    [SerializeField] private Image _cooldownMask;

    public TacticalCommand TacticalCommand => _tacticalCommand;
    private void Update() {
        if (Input.GetKeyDown(_keyCode)) {
            _toggle.isOn = !_toggle.isOn;
        }

        if (_cooldownMask == null)
            return;
        if (_tacticalCommand == TacticalCommand.RoundAttack)
        {
            if (SettlersSelectionManager.Instance.SelectedSettler.TakenTacticalCommand != null) 
            {
                _toggle.interactable = false;
                _cooldownMask.fillAmount = 1;
                return;
            }
            else
            {
                _toggle.interactable = true;
                _cooldownMask.fillAmount = 0;
            }

            if (SettlersSelectionManager.Instance.SelectedSettler.SettlerData.RoundAttackCooldown > 0)
            {
                _toggle.isOn = false;
                _toggle.interactable = false;
                _cooldownMask.fillAmount =
                    SettlersSelectionManager.Instance.SelectedSettler.SettlerData.RoundAttackCooldown /
                    Core.ConfigManager.CreaturesParametersConfig.RoundAttackCooldown;
            }
            else
            {
                _cooldownMask.fillAmount = 0;
                _toggle.interactable = true;
            }
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