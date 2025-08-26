using System.Collections.Generic;
using System.Linq;
using GameResources;
using UnityEngine;

public class ResourcesManager : IResourceManager {
    private readonly IGridService _grid;
    private List<Resource> ResourcesPrefabs;
    private Dictionary<Vector3, Resource> ExistingResourcesOnGround = new();

    public ResourcesManager(ResourcesConfig config, IGridService grid) {
        _grid = grid;
        ResourcesPrefabs = config.ResourcesPrefabs;
    }

    public void SpawnResource(Vector3 at, ResourceType type, int amount) {
        Resource prefab = GetPrefabByType(type);
        if (prefab == null) {
            Debug.LogError($"No resource prefab found for {type}");
            return;
        }

        Vector3? spawnPos = PickSpawnPos(at);
        if (spawnPos == null) {
            return;
        }

        Resource r = Object.Instantiate(prefab, spawnPos.Value, Quaternion.identity);
        ExistingResourcesOnGround.Add(spawnPos.Value, r);
        r.Init(amount);
    }

    public Resource FindResourceOnGround(ResourceType type) {
        foreach (Resource resource in ExistingResourcesOnGround.Values) {
            if (resource.Type == type) {
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

    private Vector3? PickSpawnPos(Vector3 at) {
        List<Vector3> positions = GetAroundPos(at);
        for (int i = 0; i < positions.Count; i++) {
            if (!ExistingResourcesOnGround.ContainsKey(positions[i]) && !_grid.IsOccupiedPos(at)) {
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
}