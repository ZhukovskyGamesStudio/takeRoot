using System;

public class ResourcesHelper {
    public static EquipmentType GetEquipmentByResourceType(ResourceType resourceType) {
        return resourceType switch {
            ResourceType.Hammer => EquipmentType.Hand,
            _ => EquipmentType.None
        };
    }
}