using System;

[Serializable]
public class ResourceData {
    public ResourceType ResourceType;
    public int Amount = 1;

    public static ResourceData Empty => new() {
        ResourceType = ResourceType.None,
        Amount = 0
    };
}