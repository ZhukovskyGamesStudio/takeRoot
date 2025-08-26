using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CycleTracker : MonoBehaviour {
    [SerializeField]
    private Image _image;

    [SerializeField]
    private TextMeshProUGUI _localizetext;

    [SerializeField]
    private TextMeshProUGUI _daycounter;

    public Sprite _moon, _day;

    public void Start() {
        CurrentCycle();
    }

    public void CurrentCycle() {
        //я не помню те замечательные строчки вывода с сохранений, так что вот тебе стена теста, заменишь
        _daycounter.text = "5";
    }

    public void HalveCycle(bool day) {
        day = false;
        if (day) {
            _image.sprite = _moon;
        } else {
            _image.sprite = _day;
        }
    }
}