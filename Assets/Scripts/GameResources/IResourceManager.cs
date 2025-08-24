using GameResources;
using UnityEngine;


public interface IResourceManager : IService {
	public void SpawnResource(Vector3 at, ResourceType type, int amount);
	public Resource FindResourceOnGround(ResourceType type);
}