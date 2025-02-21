using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour {
    public static SelectionManager Instance;

    [SerializeField]
    private CommandsPanel _commandsPanel;

    [SerializeField]
    private Toggle _infoToggle;

    [SerializeField]
    private InfoBookView _infoPanel;

    [SerializeField]
    private bool _autoOpenInfoPanel = false;

    public ISelectable Interactable { get; private set; }
    public ISelectable TacticalInteractable { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void Update() {
        TryAutoOpenInfoPanel();

        if (_infoToggle.isOn && _commandsPanel.SelectedCommand != Command.None) {
            _infoToggle.isOn = false;
        }
    }

    private void TryAutoOpenInfoPanel() {
        if (Input.GetMouseButton(0) && _commandsPanel.SelectedCommand == Command.None && Interactable != null) {
            if (_autoOpenInfoPanel) {
                _infoToggle.isOn = true;
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

    public void SetSelected(ISelectable obj) {
        Interactable = obj;
    }

    public void TryClearSelected(ISelectable obj) {
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