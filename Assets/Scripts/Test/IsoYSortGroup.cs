using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class IsoYSortGroup : MonoBehaviour {
    [SerializeField]
    private int _offset;

    private SortingGroup _sortingGroup;

    void Awake() {
        _sortingGroup = GetComponent<SortingGroup>();
    }

    void LateUpdate() {
        _sortingGroup.sortingOrder = 0;
    }
}