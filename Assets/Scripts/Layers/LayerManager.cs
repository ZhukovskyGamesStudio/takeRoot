using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour {
    public short currentGlobalLayer;
    private readonly List<LayerRenderer> _renderers = new();
    private readonly Dictionary<int, GameObject> _worldLayers = new();

    private void Awake() {
        ObsoleteCoreEntryPoint.LayerManager = this;
    }

    private void Start() {
        foreach (WorldLayer worldLayer in FindObjectsByType<WorldLayer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)) {
            worldLayer.InitializeLayer();
            _worldLayers.Add(worldLayer.Layer, worldLayer.gameObject);
        }

        foreach (LayerRenderer layerRenderer in FindObjectsByType<LayerRenderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)) {
            _renderers.Add(layerRenderer);
        }

        ObsoleteCoreEntryPoint.WarpManager.Init();
        SwitchWorldLayer(currentGlobalLayer);
    }

    public void MoveToAnotherLayer(short layer, HasLayer hasLayerObject) {
        hasLayerObject.layer = layer;
        hasLayerObject.transform.SetParent(_worldLayers[layer].transform, false);
        hasLayerObject.layerRenderer.SetRender(currentGlobalLayer);
    }

    public void SwitchWorldLayer(int layer) {
        currentGlobalLayer = (short)layer;
        foreach (LayerRenderer layerRenderer in _renderers) {
            layerRenderer.SetRender(currentGlobalLayer);
        }
    }
}