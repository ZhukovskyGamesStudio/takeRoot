using System.Collections.Generic;
using UnityEngine;

public class SelectionView : MonoBehaviour {
    [SerializeField]
    private Transform _lb, _lt, _rt, _rb;

    public void Init(Gridable gridable, Transform parent = null) {
        gameObject.SetActive(true);
        transform.position = gridable.GetCenterOnGrid;
        if (parent != null) {
            transform.SetParent(parent);
        }

        SetSize(gridable.GetGridEdgePoints());
    }

    private void SetSize(List<Vector3> edgePoints) {
        _lb.position = edgePoints[0];
        _lb.localScale = Vector3.one;
        _lt.position = edgePoints[1];
        _lt.localScale = Vector3.one;
        _rt.position = edgePoints[2];
        _rt.localScale = Vector3.one;
        _rb.position = edgePoints[3];
        _rb.localScale = Vector3.one;
    }

    public void Release(Transform defaltParent) {
        gameObject.SetActive(false);
        transform.SetParent(defaltParent);
    }
}