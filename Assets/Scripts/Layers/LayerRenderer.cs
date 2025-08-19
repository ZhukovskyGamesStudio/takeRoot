using System;
using UnityEngine;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(HasLayer))]
public class LayerRenderer : MonoBehaviour
{
    private HasLayer _hasLayer;

    [SerializeField]private int orderInLayer;
    
    private RendererType _rendererType;

    private SpriteRenderer _spriteRenderer;
    private Tilemap _tilemap;
    private TilemapRenderer _tilemapRenderer;
    private LayerView _layerView;

    private void Awake()
    {
        _hasLayer = GetComponent<HasLayer>();
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
        _layerView = GetComponentInChildren<LayerView>();
        if (_layerView != null)
        {
            _rendererType = RendererType.LayerView;
            return;
        }
        
        _rendererType = RendererType.None;
    }

    public void SetRender(int layer) //TODO: Remove parametr add Core.LayerManager.globalLayer
    {
        var allowRender = (_hasLayer.renderOnActualLayer && _hasLayer.layer == layer)
                          || (_hasLayer.renderOnLayersAbove && _hasLayer.layer < layer);

        var colorDiv = 1 + layer + _hasLayer.layer;
        byte colorRatio = Convert.ToByte(255 / Math.Clamp(1 + layer - _hasLayer.layer, 1, 5));
        var color = new Color32(colorRatio, colorRatio, colorRatio, 255);
        SetRenderDependsOnRendererUsed(allowRender, color);
    }

    private void SetRenderDependsOnRendererUsed(bool allowRender, Color32 color)
    {
        switch (_rendererType)
        {
            case RendererType.SpriteRenderer:
                SpriteRendererRepaint(allowRender, color);
                break;
            case RendererType.TilemapRenderer:
                TilemapRendererRepaint(allowRender, color);
                break;
            case RendererType.LayerView:
                LayerViewRepaint(allowRender, color);
                break;
                
        }
    }

    private void SpriteRendererRepaint(bool allowRender, Color32 color)
    {
        _spriteRenderer.enabled = allowRender;
        _spriteRenderer.color = color;
        _spriteRenderer.sortingOrder = (orderInLayer + _hasLayer.layer) * _hasLayer.layer;
    }

    private void TilemapRendererRepaint(bool allowRender, Color32 color)
    {
        _tilemap.color = color;
        _tilemap.gameObject.SetActive(allowRender);
        _tilemapRenderer.sortingOrder = (orderInLayer + _hasLayer.layer) * _hasLayer.layer;
    }

    private void LayerViewRepaint(bool allowRender, Color32 color)
    {
        _layerView.gameObject.SetActive(allowRender);
        _layerView.sortingGroup.sortingOrder = (orderInLayer + _hasLayer.layer) * _hasLayer.layer;
    }

}

[Serializable]
public enum RendererType
{
    SpriteRenderer,
    TilemapRenderer,
    LayerView,
    None
}