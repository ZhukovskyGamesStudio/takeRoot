using System;
using UniRx;
using UnityEngine;

public class ElectricityLevel : MonoBehaviour {
    [Header("Electricity Settings")]
    public float maxElectricity = 100;

    public bool CanDirectlyCharge = true;

    [SerializeField]
    private Progress _linkedProgress;

    [HideInInspector]
    public ReactiveProperty<float> CurrentElectricity = new();

    public bool EnoughElectricity => Mathf.Approximately(CurrentElectricity.Value, maxElectricity);

    public void ChangeElecticity(float amount) {
        CurrentElectricity.Value += amount;
        CurrentElectricity.Value = Math.Clamp(CurrentElectricity.Value, 0, maxElectricity);
        TryUpdateLinkedProgress();
    }

    private void TryUpdateLinkedProgress() {
        if (_linkedProgress) {
            _linkedProgress.ProgressData.Needed = Mathf.RoundToInt(maxElectricity);
            _linkedProgress.ProgressData.Progress = Mathf.RoundToInt(CurrentElectricity.Value);
        }
    }
}