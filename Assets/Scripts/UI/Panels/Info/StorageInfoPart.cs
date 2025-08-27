using System.Collections.Generic;
using UnityEngine;

public class StorageInfoPart : MonoBehaviour {
    [SerializeField]
    private ImageTextPair _storageSpacePrefab;

    [SerializeField]
    private Transform _storageSpacesContainer;

    [SerializeField]
    private ResourcesTable _resourcesTable;

    private StorageData  _data;
    public void SetData(StorageData data) {
        gameObject.SetActive(true);
        _data = data;

        foreach (Transform child in _storageSpacesContainer) {
            Destroy(child.gameObject);
        }

        foreach (var resourceData in data.Resources) {
            var line = Instantiate(_storageSpacePrefab, _storageSpacesContainer);
            line.SetData(_resourcesTable.ResourceIconsDictionary[resourceData.ResourceType], resourceData.Amount.ToString());
        }
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}