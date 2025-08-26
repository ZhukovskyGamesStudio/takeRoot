using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayStatusView : MonoBehaviour {
    [SerializeField]
    private Image _dayIconImage;

    [SerializeField]
    private TextMeshProUGUI _dayCounter;

    [SerializeField]
    private Sprite _moonIcon, _dayIcon;

    public void SetData(int day, bool isDay) {
        SetCurrentCycle(day);
        SetDayIcon(isDay);
    }

    private void SetCurrentCycle(int day) {
        _dayCounter.text = day.ToString();
    }

    private void SetDayIcon(bool day) {
        _dayIconImage.sprite = day ? _moonIcon : _dayIcon;
    }
}