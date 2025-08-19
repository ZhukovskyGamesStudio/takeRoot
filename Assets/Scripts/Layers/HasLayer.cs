using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HasLayer : MonoBehaviour
{
    public short layer;
    public bool renderOnActualLayer = true;
    public bool renderOnLayersAbove;
    public LayerRenderer layerRenderer;

    private void Awake()
    {
        layerRenderer = GetComponent<LayerRenderer>();
    }
}

