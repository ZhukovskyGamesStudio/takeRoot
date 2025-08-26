using UnityEngine;

public class WorldLayer : MonoBehaviour {
    public short Layer;

    public void InitializeLayer() {
        foreach (HasLayer hasLayerObject in GetComponentsInChildren<HasLayer>()) {
            hasLayerObject.layer = Layer;
        }
    }
}