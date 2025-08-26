using UnityEngine;

public class LayerGroup : MonoBehaviour {
    public bool renderOnActualLayer = true;
    public bool renderOnLayersAbove;

    public void Start() {
        HasLayer[] hasLayerObjects = GetComponentsInChildren<HasLayer>();
        foreach (HasLayer obj in hasLayerObjects) {
            obj.renderOnActualLayer = renderOnActualLayer;
            obj.renderOnLayersAbove = renderOnLayersAbove;
        }
    }
}