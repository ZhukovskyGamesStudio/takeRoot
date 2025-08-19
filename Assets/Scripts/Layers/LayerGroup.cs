using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LayerGroup : MonoBehaviour
{
    public bool renderOnActualLayer = true;
    public bool renderOnLayersAbove;
    
    public void Start()
    {
        var hasLayerObjects = GetComponentsInChildren<HasLayer>();
        foreach (HasLayer obj in hasLayerObjects)
        {
            obj.renderOnActualLayer = renderOnActualLayer;
            obj.renderOnLayersAbove = renderOnLayersAbove;
        }
    }
}