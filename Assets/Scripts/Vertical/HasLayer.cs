using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HasLayer : MonoBehaviour
{
    public int actualLayer;
    public List<int> layersAvailableForRendering;
    
    private RendererType _rendererType;
    private SpriteRenderer _spriteRenderer;
    private TilemapRenderer _tilemapRenderer;
    private Tilemap _tilemap;
    
    private void Start()
    {
        if (TryGetComponent(out _tilemapRenderer))
        {
            _rendererType = RendererType.TilemapRenderer;
            _tilemap = GetComponent<Tilemap>();
            return;
        }

        if (TryGetComponent(out _spriteRenderer))
        {
            _rendererType = RendererType.SpriteRenderer;
            return;
        }

        _rendererType = RendererType.None;
    }

    public void Init(int actualLayer, List<int> availableLayers)
    {
        this.actualLayer = actualLayer;
        layersAvailableForRendering = availableLayers;
    }
    public void Repaint(int layer)
    {
        var isAvailableLayer = layersAvailableForRendering.Contains(layer);
        var colorDiv = (1 + layer - actualLayer);
        byte colorRatio = Convert.ToByte(255 / Math.Clamp(1 + layer - actualLayer, 1, 5));
        var color = new Color32(colorRatio, colorRatio, colorRatio, 255);
        RepaintDependOnRendererUsed(isAvailableLayer, color);
    }

    private void RepaintDependOnRendererUsed(bool isAvailableLayer, Color color)
    {
        switch (_rendererType)
        {
            case RendererType.SpriteRenderer:
                SpriteRendererRepaint(isAvailableLayer, color);
                break;
            case RendererType.TilemapRenderer:
                TilemapRendererRepaint(isAvailableLayer, color);
                break;
            case RendererType.None:
                GameObjectRepaint(isAvailableLayer);
                break;
        };
    }

    private void SpriteRendererRepaint(bool isAvailableLayer, Color color)
    {
        _spriteRenderer.enabled = isAvailableLayer;
        _spriteRenderer.color = color;
    }    
    private void TilemapRendererRepaint(bool isAvailableLayer, Color color)
    {
        _tilemap.color = color;
        _tilemap.gameObject.SetActive(isAvailableLayer);
    }    
    private void GameObjectRepaint(bool isAvailableLayer)
    {
        gameObject.SetActive(isAvailableLayer);
    }
}

[Serializable]
public enum RendererType
{
    SpriteRenderer,
    TilemapRenderer,
    None
}