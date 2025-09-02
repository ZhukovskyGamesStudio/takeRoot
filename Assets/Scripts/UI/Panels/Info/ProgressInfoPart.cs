using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressInfoPart : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _titleText, _progressText;

    [SerializeField]
    private Slider _progressSlider;

    private ProgressData _progressData;
    
    public void SetData(ProgressData data) {
        gameObject.SetActive(true);
        _progressData = data;
    }

    private void UpdateData() {
        if (!_progressData.InfoViewEnabled) {
            _progressSlider.gameObject.SetActive(false);
            return;
        }
        _progressSlider.gameObject.SetActive(true);
        
        _progressSlider.value = (float)_progressData.Progress.Value / _progressData.Needed;
        _titleText.text = _progressData.Title;
        _progressText.text = $"{_progressData.Progress}/{_progressData.Needed}";
    }

    private void FixedUpdate() {
        UpdateData();
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}
