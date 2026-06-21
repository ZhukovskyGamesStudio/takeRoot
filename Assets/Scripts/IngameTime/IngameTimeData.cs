using UniRx;

public class IngameTimeData {
    public ReactiveProperty<int> Day = new ReactiveProperty<int>();
    public ReactiveProperty<float> TimeOfDayInSeconds = new ReactiveProperty<float>();
    public ReactiveProperty<float> TimeLeftToChange = new ReactiveProperty<float>();
    
    public ReactiveProperty<DayPartType> CurrentDayPartType = new ReactiveProperty<DayPartType>();
}
