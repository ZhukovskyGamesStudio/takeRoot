using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class OccurenceService : IOccurenceService, IUpdatable {
    private readonly IUpdateService _updateService;
    private readonly INetworkService _networkService;
    private List<OccurenceConfig> _occurencesConfigs;
    private OccurenceMainConfig _mainConfig;
    private float _difficltyPoints = 0;

    public Action<OccurenceConfig> OnOccurenceSpawn { get; set; }

    public OccurenceService(IConfigsProvider configsProvider, IUpdateService updateService, INetworkService networkService) {
        _updateService = updateService;
        _networkService = networkService;
        _mainConfig = configsProvider.OccurenceMainConfig;
        _occurencesConfigs = configsProvider.OccurenceConfigs;

        if (_networkService.IsHost) {
            _updateService.Register(this);
        }
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
}