using System;

[Serializable]
public enum FarmingPlantState {
    None,
    WaitingForPlanting,
    WaitingForWater,
    Growing,
    ReadyToHarvest
}
