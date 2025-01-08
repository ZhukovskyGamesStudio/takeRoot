using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamicRendererOrderSorting : MonoBehaviour {
    private List<SpriteRenderer> _spriteRenderers;

    void Awake() {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    void Update() {
        int order = Mathf.RoundToInt(-transform.position.y);
        foreach (var spriteRenderer in _spriteRenderers) {
            spriteRenderer.sortingOrder = order;
        }
    }
}