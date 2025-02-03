using System;
using UnityEngine;

public class Storagable : ECSComponent {
    [field: SerializeField]
    public int MaxStack { get; private set; } = 10;

    //TODO тут должен быть список
    public ResourceData Resource { get; private set; } = ResourceData.Empty;

    public void AddResource(ResourceData resourceData) {
        if (resourceData.ResourceType != Resource.ResourceType && !IsEmpty())
            return;
        if (IsEmpty())
            Resource.ResourceType = resourceData.ResourceType;

        var canAdd = MaxStack - Resource.Amount;
        if (resourceData.Amount <= canAdd) {
            Resource.Amount += resourceData.Amount;
            resourceData.Amount -= resourceData.Amount;
        } else {
            Resource.Amount += canAdd;
            resourceData.Amount -= canAdd;
        }
    }

    //Work in progress
    public ResourceData RemoveResource(ResourceData resourceData) {
        if (IsEmpty())
            return null;
        if (resourceData.ResourceType != Resource.ResourceType)
            return null;
        if (Resource.Amount - resourceData.Amount < 0)
            return null;

        int canRemove = Math.Min(resourceData.Amount, Resource.Amount);
        Resource.Amount -= resourceData.Amount;
        if (Resource.Amount == 0)
            Resource.ResourceType = ResourceType.None;

        var removedResource = new ResourceData {
            ResourceType = Resource.ResourceType,
            Amount = resourceData.Amount
        };

        if (IsEmpty())
            Resource.ResourceType = ResourceType.None;

        return removedResource;
    }

    public bool CanRemove(ResourceData resourceData) {
        return !IsEmpty() && resourceData.ResourceType == Resource.ResourceType;
    }

    public bool CanStore(ResourceData resourceData) {
        if (IsEmpty()) {
            return true;
        }

        return resourceData.ResourceType == Resource.ResourceType && !IsFull();
    }

    private bool IsFull() {
        return Resource.Amount == MaxStack;
    }

    private bool IsEmpty() {
        return Resource.Amount == 0;
    }

    public override int GetDependancyPriority() {
        return 0;
    }

    public override void Init(ECSEntity entity) { }
}