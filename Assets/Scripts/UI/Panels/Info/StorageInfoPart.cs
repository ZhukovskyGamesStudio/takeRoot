using System.Collections.Generic;
using UnityEngine;

public class StorageInfoPart : MonoBehaviour {
    [SerializeField]
    private ImageTextPair _storageSpacePrefab;

    [SerializeField]
    private Transform _storageSpacesContainer;

    [SerializeField]
    private ResourcesTable _resourcesTable;

    private StorageInfoData  _infoData;
    public void SetData(StorageInfoData infoData) {
        gameObject.SetActive(true);
        _infoData = infoData;

        foreach (Transform child in _storageSpacesContainer) {
            Destroy(child.gameObject);
        }

        foreach (var resourceData in infoData.Resources) {
            var line = Instantiate(_storageSpacePrefab, _storageSpacesContainer);
            line.SetData(_resourcesTable.ResourceIconsDictionary[resourceData.ResourceType], resourceData.Amount.ToString());
        }
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}