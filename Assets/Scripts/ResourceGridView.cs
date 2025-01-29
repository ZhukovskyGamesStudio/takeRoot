using System.Collections.Generic;
using UnityEngine;

public class ResourceGridView : MonoBehaviour {
    private readonly List<ResouseUiView> _cells = new List<ResouseUiView>();

    [SerializeField]
    private ResouseUiView _emptyCell;

    public void FillGrid(List<ResourceData> resources) {
        foreach (ResouseUiView uiView in _cells) {
            Destroy(uiView.gameObject);
        }

        _cells.Clear();
        gameObject.SetActive(resources.Count > 0);
        foreach (ResourceData res in resources) {
            if (res == null) {
                ResouseUiView r = Instantiate(_emptyCell, transform);
                _cells.Add(r);
            } else {
                ResouseUiView r = ResourceManager.SpawnResourceUi(res.ResourceType);
                r.transform.SetParent(transform);
                r.SetAmount(res.Amount);
                _cells.Add(r);
            }
        }
    }
}