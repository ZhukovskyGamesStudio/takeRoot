using System;
using UnityEngine;

public class Storagable : ECSComponent
{
    public int maxStack = 10;
    public ResourceData resource;
    public void AddResource(ResourceData resourceData)
    {
        if (resourceData.ResourceType != resource.ResourceType && !IsEmpty()) 
            return;
        if (IsEmpty())
            resource.ResourceType = resourceData.ResourceType;
        
        var canAdd = maxStack - resource.Amount;
        if (resourceData.Amount <= canAdd)
        {
            resource.Amount += resourceData.Amount;
            resourceData.Amount -= resourceData.Amount;
        }
        else
        {
            resource.Amount += canAdd;
            resourceData.Amount -= canAdd;
        }
    }
    
    //Work in progress
    public ResourceData RemoveResource(ResourceData resourceData)
    {
        if (IsEmpty())
            return null;
        if (resourceData.ResourceType != resource.ResourceType) 
            return null;
        if (resource.Amount - resourceData.Amount < 0)
            return null;
        
        int canRemove = Math.Min(resourceData.Amount, resource.Amount);
        resource.Amount -= resourceData.Amount;
        if (resource.Amount == 0)
            resource.ResourceType = ResourceType.None;
        
        var removedResource = new ResourceData
        {
            ResourceType = resource.ResourceType,
            Amount = resourceData.Amount
        };
        
        if (IsEmpty())
            resource.ResourceType = ResourceType.None;
        
        return removedResource;
    }

    public bool CanRemove(ResourceData resourceData)
    {
        return !IsEmpty() && resourceData.ResourceType == resource.ResourceType;
    }
    public bool CanStore(ResourceData resourceData)
    {
        return IsEmpty() && resource.ResourceType == ResourceType.None 
               || resourceData.ResourceType == resource.ResourceType && !IsFull();
    }
    
    private bool IsFull()
    {
        return resource.Amount == maxStack;
    }

    private bool IsEmpty()
    {
        return resource.Amount == 0;
    }

    public override int GetDependancyPriority() { return 0;}
    public override void Init(ECSEntity entity) { }
}