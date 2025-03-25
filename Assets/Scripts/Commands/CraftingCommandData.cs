using System;
using UnityEngine.Serialization;

[Serializable]
public class DeliveryToCraftCommandData
{ 
    public CraftingStationable CraftingStation;
}

[Serializable]
public class CraftingCommandData
{
    public CraftingStationable CraftingStation;
}