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
            string amountText =resourceData.Amount > 0 ? resourceData.Amount.ToString() : "";
            line.SetData(_resourcesTable.ResourceIconsDictionary[resourceData.ResourceType], amountText);
        }
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}