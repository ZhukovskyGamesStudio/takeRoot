using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour {
    [field: SerializeField]
    public ProgressData ProgressData { get; private set; }

    [SerializeField]
    private Slider _linkedSlider;

    private void Awake() {
        if(_linkedSlider != null) LinkSlider();
    }

    private void LinkSlider() {
        ProgressData.Progress.Subscribe(value => {
            float progress = (float)value / ProgressData.Needed;
            _linkedSlider.value = progress;
        });
    }
}
