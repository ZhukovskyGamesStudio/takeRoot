using System;
using System.Collections.Generic;
using UnityEngine;

public class FarmingPanelView : MonoBehaviour {
    [SerializeField]
    private FarmingLineView _farmingLinesPrefab;

    [SerializeField]
    private Transform _linesContainer;

    private List<FarmingPlantConfig> _farmingConfigs;
    private Action<FarmingPlantConfig> _onPlant;
    private Action _onCut;

    public void SetData(List<FarmingPlantConfig> farmingConfigs, Action<FarmingPlantConfig> onPlant, Action onCut) {
        _onPlant = onPlant;
        _onCut = onCut;
        _farmingConfigs = farmingConfigs;
        gameObject.SetActive(true);

        foreach (Transform child in _linesContainer) {
            Destroy(child.gameObject);
        }

        foreach (var config in farmingConfigs) {
            var line = Instantiate(_farmingLinesPrefab, _linesContainer);
            line.Set(config, _onPlant);
        }
    }

    public void Cut() {
        _onCut?.Invoke();
    }
}