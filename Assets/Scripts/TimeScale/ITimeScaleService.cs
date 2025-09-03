using UniRx;

public interface ITimeScaleService : IService
{
    public void SetTimeScale(GameSpeedType type, Race race);
    public ReactiveProperty<bool> IsReadyForPause { get; set; }
    public void SetReadyToPause();
    public void SetTimeMachine(TimeMachine timeMachine);
    public TimeMachine GetTimeMachine(Race race);
}
