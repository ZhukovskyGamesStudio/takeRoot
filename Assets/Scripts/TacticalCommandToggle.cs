using UnityEngine;

public class TacticalCommandToggle : MonoBehaviour
{
    [SerializeField]private TacticalCommand _tacticalCommand;
        
    [SerializeField]private TacticalCommandPanel _tacticalCommandPanel;
        
        
    public void OnValueChanged(bool val) {
        if (val) {
            _tacticalCommandPanel.SelectTacticalCommand(_tacticalCommand);
        } else {
            _tacticalCommandPanel.ClearSelectedTacticalCommand(_tacticalCommand);
        }
    }
}