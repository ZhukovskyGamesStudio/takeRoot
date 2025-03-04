using System;

[Serializable]
public class StoreCommandData {
    public ResourceView Resource;
    public Storagable TargetStorage;
}

[Serializable]
public class DeliveryCommandData {
    public BuildingPlan TargetPlan;
}