using System.Collections.Generic;
using UnityEngine;

public class SelectionView : MonoBehaviour {
    [SerializeField]
    private Transform _lb, _lt, _rt, _rb;

    private List<Transform> _poses;
    
    
    public void Init(Gridable gridable, Transform parent = null) {
        _poses ??= new List<Transform> {
            _lb, _lt, _rt, _rb
        };
        gameObject.SetActive(true);
        transform.position = gridable.GetCenterOnGrid;
        if (parent != null) {
            transform.SetParent(parent);
        }

        SetSize(gridable.GetGridEdgePoints());
    }

    private void SetSize(List<Vector3> edgePoints) {
        for (int i = 0; i < edgePoints.Count; i++) {
            _poses[i].position = edgePoints[i];
            
            Vector3 vec = _poses[i].lossyScale;
            vec.x = Mathf.Abs(_poses[i].lossyScale.x) * transform.lossyScale.x < 0 ? -1 : 1;
            _poses[i].localScale = vec;
        }
    }

    public void Release(Transform defaultParent) {
        gameObject.SetActive(false);
        transform.SetParent(defaultParent);
    }
}