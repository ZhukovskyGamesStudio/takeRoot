using System;
using UniRx;
using UnityEngine;

public class ElectricityLevel : MonoBehaviour {
    [Header("Electricity Settings")]
    public float maxElectricity = 100;

    [Header("DirectCharge Settings")]
    public bool CanDirectlyCharge = true;

    public AI.Settler ConnectedSettler;
    [SerializeField]
    public DirectChargeCable DirectChargeCable;

    public float DirectChargeSpeed = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _canDirectChargeThreshold = 0.3f;

    [field: SerializeField]
    public Transform DirectChargePos;

    public bool EnoughToDirectCharge => CurrentElectricity.Value / maxElectricity >= _canDirectChargeThreshold;

    [SerializeField]
    private Progress _linkedProgress;

    [HideInInspector]
    public ReactiveProperty<float> CurrentElectricity = new();

    public bool EnoughElectricity => Mathf.Approximately(CurrentElectricity.Value, maxElectricity);
    public bool HasElectricity => CurrentElectricity.Value > 0;

    public void ChangeElecticity(float amount) {
        CurrentElectricity.Value += amount;
        CurrentElectricity.Value = Math.Clamp(CurrentElectricity.Value, 0, maxElectricity);
        TryUpdateLinkedProgress();
    }

    private void TryUpdateLinkedProgress() {
        if (_linkedProgress) {
            _linkedProgress.ProgressData.Needed = Mathf.RoundToInt(maxElectricity);
            _linkedProgress.ProgressData.Progress.Value = Mathf.RoundToInt(CurrentElectricity.Value);
        }
    }

    public void DecreaseElectricity(float amount) {
        ChangeElecticity(-amount);
    }
}