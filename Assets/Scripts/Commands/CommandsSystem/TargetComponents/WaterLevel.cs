using System;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class WaterLevel : MonoBehaviour {
    [FormerlySerializedAs("maxMoisture"), Header("Water Settings")]
    public float maxWater = 1;

    [HideInInspector]
    public ReactiveProperty<float> CurrentWater = new();

    [SerializeField]
    private Progress _linkedProgress;

    public bool EnoughWater => Mathf.Approximately(CurrentWater.Value, maxWater);

    public void Dry(float amount) {
        CurrentWater.Value -= amount;
        TryUpdateLinkedProgress();
    }

    public void ChangeWater(float amount) {
        CurrentWater.Value += amount;
        CurrentWater.Value = Math.Clamp(CurrentWater.Value, 0, maxWater);
        TryUpdateLinkedProgress();
    }

    private void TryUpdateLinkedProgress() {
        if (_linkedProgress) {
            _linkedProgress.ProgressData.Needed = Mathf.RoundToInt(maxWater);
            _linkedProgress.ProgressData.Progress = Mathf.RoundToInt(CurrentWater.Value);
        }
    }
}