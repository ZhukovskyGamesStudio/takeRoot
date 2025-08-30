using System;
using UnityEngine;

[RequireComponent(typeof(WaterLevel))]
public class FarmingPlot : MonoBehaviour {
    [SerializeField]
    public float DryThreshold = 0.3f;

    [SerializeField]
    public float GrowThreshold = 1f;

    [SerializeField]
    private SpriteRenderer _plotView, _plantView;

    [SerializeField]
    private Sprite _wateredSprite, _drySprite;

    [HideInInspector]
    public FarmingPlantType PlantType;

    [HideInInspector]
    public FarmingPlantState PlantState;

    private WaterLevel _waterLevel;

    public float GrowingLevel;

    public void Init() {
        _waterLevel = GetComponent<WaterLevel>();
        PlantType = FarmingPlantType.None;
        PlantState = FarmingPlantState.None;
        _waterLevel.ChangeWater(1);
        GrowingLevel = 0;
    }

    public void ChangeWaterLevel(float amount) {
        _waterLevel.ChangeWater(amount);

        if (_waterLevel.currentWater <= DryThreshold) {
            _plotView.sprite = _drySprite;
        } else {
            _plotView.sprite = _wateredSprite;
        }
    }

    public void ChangeGrow(float amount) {
        
        if (PlantState == FarmingPlantState.Growing) {
            GrowingLevel += amount;
            GrowingLevel = Mathf.Clamp01(GrowingLevel);
        }

        if (GrowingLevel >= GrowThreshold) {
            PlantState = FarmingPlantState.ReadyToHarvest;
        }
    }

    public bool NeedsWatering() => PlantType != FarmingPlantType.None && PlantState != FarmingPlantState.WaitingForWater;
}