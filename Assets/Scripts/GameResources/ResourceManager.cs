using System.Collections.Generic;
using System.Linq;
using GameResources;
using Settlers.Infrastructure;
using UnityEngine;

public class ResourcesManager : IResourceManager{
	private List<Resource> ResourcesPrefabs;
	private Dictionary<Vector3, Resource> ResourcesPositions = new Dictionary<Vector3, Resource>();

	public ResourcesManager(ResourcesConfig config) {
		ResourcesPrefabs = config.ResourcesPrefabs;
	}
	public void SpawnResource(Vector3 at, ResourceType type, int amount) {
		var prefab = GetPrefabByType(type);
		if (prefab == null) return;
		while (ResourcesPositions.ContainsKey(at)) {
			at += new Vector3( 1, 0, 0);
		}
		Resource r = Object.Instantiate(prefab, at, Quaternion.identity);
		ResourcesPositions.Add(at, r);
		r.Init(amount);
	}
	public Resource GetPrefabByType(ResourceType type) {
		return ResourcesPrefabs.FirstOrDefault(r => r.Type == type);
	}
}