using System;
using System.Collections.Generic;
using System.Linq;
using GameResources;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ResourcesManager : IResourceManager {
    private readonly ResourcesConfig _config;
    private readonly ResourcesTable _config2;
    private readonly IGridService _grid;
    private readonly INetworkService _networkService;
    private List<Resource> ResourcesPrefabs ;
    private Dictionary<Vector3, Resource> ExistingResourcesOnGround = new();

    public ResourcesManager(IConfigsProvider configService, IGridService grid, INetworkService networkService) {
        _config = configService.ResourcesConfig;
        _config2 = configService.ResourcesTable;
        _grid = grid;
        _networkService = networkService;
        ResourcesPrefabs = _config.ResourcesPrefabs;
    }

    public void SpawnResource(Vector3 at, ResourceType type, int amount) {
        Resource prefab = GetPrefabByType(type);
        if (prefab == null) {
            Debug.LogError($"No resource prefab found for {type}");
            return;
        }

        Vector3? spawnPos = PickSpawnPos(at);
        if (spawnPos == null) {
            Debug.LogError($"No pos found found for {type}");
            return;
        }
        spawnPos = new Vector3(Mathf.Ceil(spawnPos.Value.x), Mathf.Ceil(spawnPos.Value.y), spawnPos.Value.z);
        Resource r = _networkService.InstantiateAndSpawn(prefab, spawnPos.Value);
        ExistingResourcesOnGround.Add(spawnPos.Value, r);
        r.Init(amount);
    }

    public void DestroyResource(Vector3 at) {
        var resource = ExistingResourcesOnGround[at];
        ExistingResourcesOnGround.Remove(at);
        Object.Destroy(resource.gameObject);
    }

    public Resource FindResourceOnGround(ResourceType type) {
        foreach (Resource resource in ExistingResourcesOnGround.Values) {
            if (resource.Type == type && resource.Amount - resource.Reserved > 0) {
                return resource;
            }
        }

        return null;
    }

    //TODO optimize this
    public Dictionary<ResourceType, int> TotalResources() {
        Dictionary<ResourceType, int> totalResources = new();
        foreach (KeyValuePair<Vector3, Resource> resource in ExistingResourcesOnGround) {
            if (!totalResources.TryAdd(resource.Value.Type, resource.Value.Amount)) {
                totalResources[resource.Value.Type] += resource.Value.Amount;
            }
        }

        return totalResources;
    }

    public Sprite GetResourceSpriteNoShadow(ResourceType type) => _config2.ResourceIconsDictionary[type];

    private Vector3? PickSpawnPos(Vector3 at) {
        List<Vector3> positions = GetAroundPos(at);
        for (int i = 0; i < positions.Count; i++) {
            var pos = new Vector3(Mathf.Ceil(positions[i].x), Mathf.Ceil(positions[i].y), positions[i].z);
            if (!ExistingResourcesOnGround.ContainsKey(pos) && !_grid.IsOccupiedPos(pos)) {
                return positions[i];
            }
        }

        return null;
    }

    private List<Vector3> GetAroundPos(Vector3 origin) {
        return new List<Vector3> {
            origin,
            origin + new Vector3(1, 0),
            origin + new Vector3(1, -1),
            origin + new Vector3(0, -1),
            origin + new Vector3(-1, -1),
            origin + new Vector3(-1, 0),
            origin + new Vector3(-1, 1),
            origin + new Vector3(0, 1),
            origin + new Vector3(1, 1)
        };
    }

    private Resource GetPrefabByType(ResourceType type) {
        return ResourcesPrefabs.FirstOrDefault(r => r.Type == type);
    }
    public void SpawnRandomResources(Vector3 at, int amount) {
        Dictionary<ResourceType, int> totalResources = new Dictionary<ResourceType, int>();
        for (int i = 0; i < amount; i++) {
            var type = GetRandomResourceType();
            if (totalResources.TryGetValue(type, out _)) {
                totalResources[type]++;
            } else {
                totalResources.Add(type, 1);
            }
        }
        foreach (var kvp in totalResources) {
            var type = kvp.Key;
            var amountToSpawn = kvp.Value;
            SpawnResource(at, type, amountToSpawn);
        }

    }

    private ResourceType GetRandomResourceType() {
        var total = _config.TotalWeight;
        var point = 0;
        var n = Random.Range(0, total);
        foreach (var kvp in _config.ResourceWeights) {
            var type = kvp.Key;
            var weight = kvp.Value;
            point += weight;
            if (n < point) {
                return type;
            }
        }
        return ResourceType.None;
    }
}