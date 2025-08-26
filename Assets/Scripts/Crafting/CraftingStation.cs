using System;
using System.Collections.Generic;
using CodeBase.Services;
using UnityEngine;

public class CraftingStation : MonoBehaviour {
    public Dictionary<ResourceType, int> RequiredResources;
    public Dictionary<ResourceType, int> ResourceStorage;

    private void Start() {
        ServiceLocator.Container.Single<ICraftingService>().AddCraftingStation(this);
        RequiredResources = new Dictionary<ResourceType, int>();
        ResourceStorage = new Dictionary<ResourceType, int>();
        foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
            if (type == ResourceType.None) {
                continue;
            }

            RequiredResources[type] = 0;
            ResourceStorage[type] = 0;
        }

        RequiredResources[ResourceType.Planks] = 5;
    }

    public ResourceType GetRequiredResource() {
        foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
            if (type == ResourceType.None) {
                continue;
            }

            if (RequiredResources[type] == 0) {
                continue;
            }

            if (ResourceStorage[type] < RequiredResources[type]) {
                return type;
            }
        }

        return ResourceType.None;
    }

    public void StoreResource(ResourceType type, int amount) {
        ResourceStorage[type] += amount;
    }
}