using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ResourcesView : MonoBehaviour {
    [SerializeField]
    private int _maxVisibleResources = 10;

    [SerializeField]
    private Button _upButton, _downButton;

    [SerializeField]
    private ResourceLineView _resorceLinePrefab;

    [SerializeField]
    private Transform _resourcesContainer;

    private void Start() {
        InitMockData();
    }

    private void InitMockData() {
        SetData(new AYellowpaper.SerializedCollections.SerializedDictionary<ResourceType, int>() {
            { ResourceType.MetalScraps, Random.Range(1, 99) },
            { ResourceType.Biofuel, Random.Range(1, 99) },
            { ResourceType.MashedPotato, Random.Range(1, 99) },
            { ResourceType.CleanedMetal, Random.Range(1, 99) },
            { ResourceType.CleanedPlank, Random.Range(1, 99) },
            { ResourceType.EmptyBottle, Random.Range(1, 99) },
            { ResourceType.Planks, Random.Range(1, 99) },
            { ResourceType.Potato, Random.Range(1, 99) },
        });
    }

    public void SetData(AYellowpaper.SerializedCollections.SerializedDictionary<ResourceType, int> dictionary) {
        var isTooLong = dictionary.Count >= _maxVisibleResources;
        _upButton.gameObject.SetActive(isTooLong);
        _downButton.gameObject.SetActive(isTooLong);
        foreach (Transform child in _resourcesContainer) {
            Destroy(child.gameObject);
        }

        foreach (var item in dictionary) {
            var line = Instantiate(_resorceLinePrefab, _resourcesContainer);
            line.SetData(item.Key, item.Value);
        }
    }
}