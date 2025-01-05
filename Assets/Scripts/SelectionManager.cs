using System;
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
    public InteractableObject InteractableObject { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void SetSelected(InteractableObject obj) {
        InteractableObject = obj;
    }

    public void TryClearSelected(InteractableObject obj) {
        if (InteractableObject == obj) {
            InteractableObject = null;
        }
    }

    public void Update() {
        if (Input.GetMouseButton(0) && _commandsPanel.SelectedCommand == Command.None && InteractableObject != null) {
            _infoToggle.isOn = true;
            _infoPanel.Init(InteractableObject.GetInfoFunc?.Invoke());
        }

        if (_infoToggle.isOn && _commandsPanel.SelectedCommand != Command.None) {
            _infoToggle.isOn = false;
        }
    }
}