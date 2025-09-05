using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchView : MonoBehaviour {
    [SerializeField]
    private AYellowpaper.SerializedCollections.SerializedDictionary<Graphic, Color> _startedColors, _inProcessColors, _completedColors;

    private AYellowpaper.SerializedCollections.SerializedDictionary<Graphic, Color> _defaultColors = new();

    [SerializeField]
    private Image _bg;

    [SerializeField]
    private Sprite _startedBg, _inProcessBg, _completedBg;

    [SerializeField]
    private TextMeshProUGUI _titleText, _progressText;

    [SerializeField]
    private Button _researchButton;

    [SerializeField]
    private Slider _progressSlider;

    [SerializeField]
    private CanvasGroup _disabledGroup;

    [SerializeField]
    private Transform _rewardsContainer;

    [field: SerializeField]
    public Research Id { get; private set; }

    private ResearchData _data;
    private Sprite _defaultBg;

    public void InitData(ResearchData data, ResearchPanelView panelView) {
        _data = data;
        _defaultBg = _bg.sprite;

        _titleText.text = data.DisplayName;
        _progressText.text = data.Price.ToString();
        if (data.IsDisabled) {
            _researchButton.interactable = false;
        }
        
        _researchButton.onClick.AddListener(() => panelView.SelectResearch(_data));

        foreach (KeyValuePair<Graphic, Color> pair in _startedColors) {
            _defaultColors[pair.Key] = pair.Key.color;
        }

        foreach (SpriteAndName reward in data.Rewards) {
            Image image = new GameObject().AddComponent<Image>();
            image.transform.SetParent(_rewardsContainer);
            image.sprite = reward.Sprite;
            image.preserveAspect = true;
        }
    }

    public void UpdateData(int progress, bool inProcess) {
        _progressSlider.value = (float)progress / _data.Price;
        _disabledGroup.enabled = !_data.Researchable;
        
        if (progress == _data.Price) {
            ApplyColors(_completedColors);
            _progressText.text = "изучено";
            _bg.sprite = _completedBg;
        } else if (inProcess) {
            ApplyColors(_inProcessColors);
            _bg.sprite = _inProcessBg;
        } else if (progress > 0) {
            ApplyColors(_startedColors);
            _bg.sprite = _startedBg;
        } else {
            ApplyColors(_defaultColors);
            _bg.sprite = _defaultBg;
        }
    }

    private void ApplyColors(AYellowpaper.SerializedCollections.SerializedDictionary<Graphic, Color> colors) {
        foreach (KeyValuePair<Graphic, Color> pair in colors) {
            pair.Key.color = pair.Value;
        }
    }
}