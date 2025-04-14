using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour {

    [SerializeField]
    private CommandsPanel _commandsPanel;

    [SerializeField]
    private Toggle _infoToggle;

    [SerializeField]
    private InfoBookView _infoPanel;

    [SerializeField]
    private bool _autoOpenInfoPanel = false;

    public Interactable Interactable { get; private set; }
    public ISelectable TacticalInteractable { get; private set; }

    private void Awake() {
        Core.SelectionManager = this;
    }

    public void Update() {
        TryAutoOpenInfoPanel();

        if (_infoToggle.isOn && _commandsPanel.SelectedCommand != Command.None) {
            _infoToggle.isOn = false;
        }
    }

    private void TryAutoOpenInfoPanel() {
        if (Input.GetMouseButtonDown(0) && _commandsPanel.SelectedCommand == Command.None && Interactable != null) {
            if (_autoOpenInfoPanel) {
                _infoToggle.isOn = true;
            }

            if (Interactable.TryGetComponent(out CraftingStationable craftingStationable))
            {
                _infoPanel.Init(craftingStationable);
                return;
            }
            
            _infoPanel.Init(Interactable.GetInfoData());
        }

        if (Input.GetMouseButton(0) && _commandsPanel.SelectedCommand == Command.None && TacticalInteractable != null) {
            if (_autoOpenInfoPanel) {
                _infoToggle.isOn = true;
            }

            _infoPanel.Init(TacticalInteractable.GetInfoData());
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