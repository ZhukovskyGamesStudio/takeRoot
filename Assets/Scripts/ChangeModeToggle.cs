using UnityEngine;
using UnityEngine.UI;

public class ChangeModeToggle : MonoBehaviour
{
    [SerializeField]private CommandsPanel _commandsPanel;
    [SerializeField]private TacticalCommandPanel _tacticalCommandPanel;
    public void OnValueChanged(bool val)
    {
        var settler = SettlersSelectionManager.Instance.SelectedSettler;
        if (val) {
            settler.ChangeMode(Mode.Tactical);
            
        } else {
            settler.ChangeMode(Mode.Planning);
        }
    }
}