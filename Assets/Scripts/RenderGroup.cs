using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class RenderGroup : MonoBehaviour {
    [SerializeField]
    private List<SpriteRenderer> _spriteRenderers;

    public bool IsUpdating;

    [Range(0, 1)]
    public float AlphaValue = 1;

    private void Start() {
        _spriteRenderers = transform.GetComponentsInChildren<SpriteRenderer>(true).ToList();
    }

    private void LateUpdate() {
        if (!IsUpdating) {
            return;
        }

        foreach (SpriteRenderer spriteRenderer in _spriteRenderers) {
            Color color = spriteRenderer.color;
            color.a = AlphaValue;
            spriteRenderer.color = color;
        }
    }
    
    
}