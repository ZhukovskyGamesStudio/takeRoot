using System;
using UnityEngine;

public class SelectionManager : MonoBehaviour {
    public static SelectionManager Instance;

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
}