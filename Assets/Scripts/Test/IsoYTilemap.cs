using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
public class IsoYTilemap : MonoBehaviour {
    [SerializeField]
    private int _offset;

    private TilemapRenderer _tilemapRenderer;

    void Awake() {
        _tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    void LateUpdate() {
        _tilemapRenderer.sortingOrder = 0;
    }
}