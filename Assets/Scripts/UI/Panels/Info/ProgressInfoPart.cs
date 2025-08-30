using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressInfoPart : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _titleText, _progressText;

    [SerializeField]
    private Slider _progressSlider;
    
    public void SetData(ProgressData data) {
        gameObject.SetActive(true);

        _progressSlider.value = (float)data.Progress / data.Needed;
        _titleText.text = data.Title;
        _progressText.text = $"{data.Progress}/{data.Needed}";
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}
