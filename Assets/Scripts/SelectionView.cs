using System.Collections.Generic;
using UnityEngine;

public class SelectionView : MonoBehaviour {
    [SerializeField]
    private Transform _lb, _lt, _rt, _rb;

    public void Init(Gridable gridable) {
        gameObject.SetActive(true);
        transform.position = gridable.GetCenterOnGrid;
        transform.SetParent(gridable.transform);
        SetSize(gridable.GetGridEdgePoints());
    }

    private void SetSize(List<Vector3> edgePoints) {
        _lb.position = edgePoints[0];
        _lt.position = edgePoints[1];
        _rt.position = edgePoints[2];
        _rb.position = edgePoints[3];
    }

    public void Release(Transform defaltParent) {
        gameObject.SetActive(false);
        transform.SetParent(defaltParent);
    }
}