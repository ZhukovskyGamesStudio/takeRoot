using System.Collections.Generic;
using GameResources;
using UnityEngine;

public interface IResourceManager : IService {
    public void SpawnResource(Vector3 at, ResourceType type, int amount);
    public void SpawnRandomResources(Vector3 at, int amount);
    public void DestroyResource(Vector3 at);
    public Resource FindResourceOnGround(ResourceType type);
    public Dictionary<ResourceType, int> TotalResources();

    public Sprite GetResourceSpriteNoShadow(ResourceType type);
}