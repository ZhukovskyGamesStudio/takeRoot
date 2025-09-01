using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class OccurenceService : IOccurenceService, IUpdatable {
    private readonly IUpdateService _updateService;
    private List<OccurenceConfig> _occurencesConfigs;
    private OccurenceMainConfig _mainConfig;
    private float _difficltyPoints = 0;

    public OccurenceService(IConfigsProvider configsProvider, IUpdateService updateService) {
        _updateService = updateService;
        _mainConfig = configsProvider.OccurenceMainConfig;
        _occurencesConfigs = configsProvider.OccurenceConfigs;

        _updateService.Register(this);
    }

    public void Update() {
        _difficltyPoints += _mainConfig.DifficultyPointsPerMinute / 60 * Time.deltaTime;
        TrySpawnOccurence();
    }
    
    private void TrySpawnOccurence() {
        foreach (var occurenceConfig in _occurencesConfigs) {
            if (_difficltyPoints >= occurenceConfig.DifficultyCost) {
                SpawnOccurence(occurenceConfig);
                _difficltyPoints -= occurenceConfig.DifficultyCost;
                break;
            }
        }
    }

    private void SpawnOccurence(OccurenceConfig occurenceConfig) {
        Object.Instantiate(occurenceConfig.PrefabToSpawn);
        OnOccurenceSpawn?.Invoke(occurenceConfig);
    }

    public void Dispose() {
        _updateService.Unregister(this);
    }

    public Action<OccurenceConfig> OnOccurenceSpawn { get; set; }
}