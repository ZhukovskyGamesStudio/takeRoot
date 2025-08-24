using System.Collections.Generic;
using System.Linq;
using GameResources;
using UnityEngine;

public class ResourcesManager : IResourceManager{
	private List<Resource> ResourcesPrefabs;
	private Dictionary<Vector3, Resource> ExistingResourcesOnGround = new Dictionary<Vector3, Resource>();

	public ResourcesManager(ResourcesConfig config) {
		ResourcesPrefabs = config.ResourcesPrefabs;
	}
	public void SpawnResource(Vector3 at, ResourceType type, int amount) {
		var prefab = GetPrefabByType(type);
		if (prefab == null) {
			Debug.LogError($"No resource prefab found for {type}");
			return;
		}
		
		Vector3? spawnPos = PickSpawnPos(at);
		if (spawnPos == null) return;
		
		Resource r = Object.Instantiate(prefab, spawnPos.Value, Quaternion.identity);
		ExistingResourcesOnGround.Add(spawnPos.Value, r);
		r.Init(amount);
	}

	private Vector3? PickSpawnPos(Vector3 at) {
		var positions = GetAroundPos(at);
		for (int i = 0; i < positions.Count; i++) {
			if (!ExistingResourcesOnGround.ContainsKey(positions[i])) {
				return positions[i];
			}
		}
		return null;
	}

	private List<Vector3> GetAroundPos(Vector3 origin) {
		return new List<Vector3>() {
			origin,
			origin + new Vector3(1, 0),
			origin + new Vector3(1, -1),
			origin + new Vector3(0, -1),
			origin + new Vector3(-1, -1),
			origin + new Vector3(-1, 0),
			origin + new Vector3(-1, 1),
			origin + new Vector3(0, 1),
			origin + new Vector3(1, 1),
		};
	}

	private Resource GetPrefabByType(ResourceType type) {
		return ResourcesPrefabs.FirstOrDefault(r => r.Type == type);
	}
}