using System;
using UnityEngine;
using UnityEngine.Serialization;

public class WaterLevel : MonoBehaviour {
    [FormerlySerializedAs("maxMoisture"), Header("Water Settings")] 
    public float maxWater;

    [FormerlySerializedAs("currentMoisture")]
    public float currentWater;

    public bool EnoughWater => Mathf.Approximately(currentWater, maxWater);

    public void Dry(float amount) {
        currentWater -= amount;
    }

    public void Water(float amount) {
        currentWater += amount;
        currentWater = Math.Clamp(currentWater, 0, maxWater);
    }
}