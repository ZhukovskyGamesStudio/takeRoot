using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    public int CurrentLayer;
    private List<HasLayer> objs = new List<HasLayer>();
    private void Start()
    {
        foreach (HasLayer repainter in FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).OfType<HasLayer>())
        {
            objs.Add(repainter);
        }
    }

    public void ChangeLayer(int layer)
    {
        CurrentLayer = layer;
        foreach (HasLayer repainter in objs)
        {
            repainter.Repaint(CurrentLayer);
        }
    }
}