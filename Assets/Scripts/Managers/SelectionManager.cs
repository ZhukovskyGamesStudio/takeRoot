using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour {

    [SerializeField]
    private CommandsPanel _commandsPanel;

    [SerializeField]
    private bool _autoOpenInfoPanel;

    public Interactable Interactable { get; private set; }
    public ISelectable TacticalInteractable { get; private set; }

    private void Awake() {
        Core.SelectionManager = this;
    }

    public void Update() {
        TryAutoOpenInfoPanel();
    }

    private void TryAutoOpenInfoPanel() {
        if (Input.GetMouseButtonDown(0) && _commandsPanel.SelectedCommand == Command.None) {
            
            if(Interactable != null) Core.UI.OpenInfoPanel(Interactable);
            else if (TacticalInteractable != null) {
                if (TacticalInteractable.GetGameObject().GetComponent<SettlerData>() != null) return;
                
                Core.UI.OpenInfoPanel(TacticalInteractable);
            }
        }
    }

    public void SetSelected(Interactable obj) {
        Interactable = obj;
    }

    public void TryClearSelected(Interactable obj) {
        if (Interactable == obj) {
            Interactable = null;
        }
    }

    public void SetTacticalSelected(ISelectable obj) {
        TacticalInteractable = obj;
    }

    public void TryClearTacticalSelected(ISelectable obj) {
        if (TacticalInteractable == obj) {
            TacticalInteractable = null;
        }
    }
}