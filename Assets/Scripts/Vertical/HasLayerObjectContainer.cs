using System.Collections.Generic;
using UnityEngine;

public class HasLayerObjectContainer : MonoBehaviour
{
    public int actualLayer;
    public List<int> availableForRenderLayers;

    public void Start()
    {
        var hasLayerObjects = GetComponentsInChildren<HasLayer>();
        foreach (HasLayer obj in hasLayerObjects)
        {
            obj.Init(actualLayer, availableForRenderLayers);
        }
    }
}