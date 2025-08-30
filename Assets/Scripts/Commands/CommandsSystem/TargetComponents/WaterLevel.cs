using System;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class WaterLevel : MonoBehaviour {
    [FormerlySerializedAs("maxMoisture"), Header("Water Settings")] 
    public float maxWater = 1;

    [FormerlySerializedAs("currentMoisture")]
    public ReactiveProperty<float> currentWater = new ReactiveProperty<float>();

    public bool EnoughWater => Mathf.Approximately(currentWater.Value, maxWater);

    public void Dry(float amount) {
        currentWater.Value -= amount;
    }

    public void ChangeWater(float amount) {
        currentWater.Value += amount;
        currentWater.Value = Math.Clamp(currentWater.Value, 0, maxWater);
    }
}