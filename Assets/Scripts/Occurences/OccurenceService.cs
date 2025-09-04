using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class OccurenceService : IOccurenceService, IUpdatable {
    private readonly IUpdateService _updateService;
    private readonly ISettlersService _settlers;
    private readonly IGridService _grid;
    private readonly INetworkService _networkService;
    private List<OccurenceConfig> _occurencesConfigs;
    private OccurenceMainConfig _mainConfig;
    private float _difficltyPoints = 0;
    private int _minDistanceFromSettlers = 10;

    public OccurenceService(IConfigsProvider configsProvider, IUpdateService updateService, ISettlersService settlers, IGridService grid,
        INetworkService networkService) {
        _updateService = updateService;
        _settlers = settlers;
        _grid = grid;
        _networkService = networkService;
        _mainConfig = configsProvider.OccurenceMainConfig;
        _occurencesConfigs = configsProvider.OccurenceConfigs;

        if (networkService.IsHost) {
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
        var pos = PickSpawnPos();
        _networkService.InstantiateAndSpawn(occurenceConfig.GetRandomizeToSpawn.GetComponent<NetworkBehaviour>(), pos);
        OnOccurenceSpawn?.Invoke(occurenceConfig);
    }

    private Vector3 PickSpawnPos() {
        Vector3 pos;
        var settlers = _settlers.AllSettlers();
        var minXR = settlers.Max(s => s.transform.position.x) + _minDistanceFromSettlers;
        var minXL = settlers.Min(s => s.transform.position.x) - _minDistanceFromSettlers;
        var minYT = settlers.Max(s => s.transform.position.y) + _minDistanceFromSettlers;
        var minYB = settlers.Min(s => s.transform.position.y) - _minDistanceFromSettlers;
        var distanceX = 25;
        var distanceY = 20;
        while (true) {
            bool isLeft = Random.Range(0, 2) == 0;
            bool isBottom = Random.Range(0, 2) == 0;
            var x = isLeft ? Random.Range(minXL - distanceX, minXL) : Random.Range(minXR, minXR + distanceX);
            var y = isBottom ? Random.Range(minYB - distanceY, minYB) : Random.Range(minYT, minYT + distanceY);
            pos = new Vector3(x, y, 0);
            if (!_grid.OnMap(pos) || _grid.IsOccupiedPos(pos)) {
                distanceX--;
                distanceY--;
            } else
                break;
        }

        return pos;
    }

    public void Dispose() {
        _updateService.Unregister(this);
    }

    public Action<OccurenceConfig> OnOccurenceSpawn { get; set; }
}