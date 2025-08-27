using System.Collections.Generic;
using UnityEngine;

public class StorageInfoPart : MonoBehaviour {
    [SerializeField]
    private ImageTextPair _storageSpacePrefab;

    [SerializeField]
    private Transform _storageSpacesContainer;

    [SerializeField]
    private ResourcesTable _resourcesTable;

    public void SetData(List<ResourceData> resourceDatas) {
        gameObject.SetActive(true);

        foreach (Transform child in _storageSpacesContainer) {
            Destroy(child.gameObject);
        }

        foreach (var resourceData in resourceDatas) {
            var line = Instantiate(_storageSpacePrefab, _storageSpacesContainer);
            line.SetData(_resourcesTable.ResourceIconsDictionary[resourceData.ResourceType], resourceData.Amount.ToString());
        }
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}