using System;
using UnityEngine;

[RequireComponent(typeof(HasLayer))]
public class TransformPoint : MonoBehaviour
{
    private HasLayer _hasLayer;
    
    public Point Point => new(transform.localPosition.ToVector2Int().x, transform.localPosition.ToVector2Int().y, _hasLayer.layer);
    
    private void Awake()
    {
        _hasLayer = GetComponent<HasLayer>();
    }
}