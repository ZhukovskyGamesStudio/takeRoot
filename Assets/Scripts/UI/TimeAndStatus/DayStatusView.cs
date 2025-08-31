using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class DayStatusView : MonoBehaviour {
    [SerializeField]
    private Image _dayIconImage;

    [SerializeField]
    private TextMeshProUGUI _dayCounter, _timeUntilChange;

    [SerializeField]
    private Sprite _moonIcon, _dayIcon;

    public void SetData(IngameTimeData data) {
        data.Day.Subscribe(SetCurrentCycle);
        data.CurrentDayPartType.Subscribe(SetDayIcon);
        data.TimeLeftToChange.Subscribe(SetTimeUntilChange);
        
        SetCurrentCycle(data.Day.Value);
        SetDayIcon(data.CurrentDayPartType.Value);
        SetTimeUntilChange(data.TimeLeftToChange.Value);
    }

    private void SetCurrentCycle(int day) {
        _dayCounter.text = day.ToString();
    }

    private void SetDayIcon(DayPartType day) {
        _dayIconImage.sprite = day == DayPartType.Daytime ? _moonIcon : _dayIcon;
    }
    
    private void SetTimeUntilChange(float time) {
        var minutes = Mathf.FloorToInt(time / 60);
        var seconds = Mathf.FloorToInt(time % 60);
        _timeUntilChange.text = $"{minutes:00}:{seconds:00}";
    }
}