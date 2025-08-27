using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceGridView : MonoBehaviour {
    private readonly List<ResourseUiView> _cells = new();

    [SerializeField]
    private ResourseUiView _emptyCell;

    public void FillGrid(List<ResourceData> resources) {
        foreach (ResourseUiView uiView in _cells) {
            Destroy(uiView.gameObject);
        }

        _cells.Clear();
        gameObject.SetActive(resources.Count > 0);
        foreach (ResourceData res in resources) {
            if (res == null) {
                ResourseUiView r = Instantiate(_emptyCell, transform);
                _cells.Add(r);
            } else {
                ResourseUiView r = ResourceManager.SpawnResourceUi(res.ResourceType);
                r.transform.SetParent(transform);
                r.SetAmount(res.Amount);
                _cells.Add(r);
            }
        }
    }

    public ResourseUiView GetResourceView(ResourceType type) {
        return _cells.FirstOrDefault(r => r.ResourceType == type);
    }
}