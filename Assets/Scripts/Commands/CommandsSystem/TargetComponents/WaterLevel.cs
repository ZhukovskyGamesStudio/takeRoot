using System;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class WaterLevel : NetworkBehaviour {
    [FormerlySerializedAs("maxMoisture"), Header("Water Settings")]
    public float maxWater = 1;

    [HideInInspector]
    public ReactiveProperty<float> CurrentWater = new();

    [SerializeField]
    private Progress _linkedProgress;

    public bool EnoughWater => Mathf.Approximately(CurrentWater.Value, maxWater);

    [ServerRpc]
    public void DryServerRpc(float amount) {
        CurrentWater.Value -= amount;
        TryUpdateLinkedProgress();
        SyncWaterClientRpc(CurrentWater.Value);
    }

    [ServerRpc]
    public void ChangeWaterServerRpc(float amount) {
        CurrentWater.Value += amount;
        CurrentWater.Value = Math.Clamp(CurrentWater.Value, 0, maxWater);
        TryUpdateLinkedProgress();

        SyncWaterClientRpc(CurrentWater.Value);
    }

    [ClientRpc]
    private void SyncWaterClientRpc(float amount) {
        CurrentWater.Value = amount;
        TryUpdateLinkedProgress();
    }

    private void TryUpdateLinkedProgress() {
        if (_linkedProgress) {
            _linkedProgress.ProgressData.Needed = Mathf.RoundToInt(maxWater);
            _linkedProgress.ProgressData.Progress.Value = Mathf.RoundToInt(CurrentWater.Value);
        }
    }
}