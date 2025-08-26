using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchView : MonoBehaviour
{
    [SerializeField]
    private AYellowpaper.SerializedCollections.SerializedDictionary<Graphic, Color> _startedColors, _inProcessColors, _completedColors;

    private AYellowpaper.SerializedCollections.SerializedDictionary<Graphic, Color> _defaultColors = new();
    
    [SerializeField]
    private GameObject _bg;

    [SerializeField]
    private TextMeshProUGUI _titleText, _progressText;

    [field: SerializeField]
    public Research Id { get; private set; }

    private ResearchData _data;

    public void InitData(ResearchData data) {
        _data = data;
        
        _titleText.text = data.DisplayName;
        _progressText.text = data.Price.ToString();
        
        foreach (KeyValuePair<Graphic, Color> pair in _startedColors) {
            _defaultColors[pair.Key] = pair.Key.color;
        }
    }

    public void UpdateData(int progress, bool inProcess) {
        _bg.SetActive(!inProcess && progress == 0);

        if (progress == _data.Price) {
            ApplyColors(_completedColors);
            _progressText.text = "изучено";
        } else if (inProcess) {
            ApplyColors(_inProcessColors);
        } else if (progress > 0) {
            ApplyColors(_startedColors);
        } else {
            ApplyColors(_defaultColors);
        }
    }
    
    private void ApplyColors(AYellowpaper.SerializedCollections.SerializedDictionary<Graphic, Color> colors) {
        foreach (KeyValuePair<Graphic, Color> pair in colors) {
            pair.Key.color = pair.Value;
        }
    }
}
