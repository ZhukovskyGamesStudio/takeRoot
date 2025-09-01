public interface ITimeScaleService : IService
{
    public void SetTimeScale(GameSpeedType type);
    public void SetReadyToPause();
    public void SetTimeMachine(TimeMachine timeMachine);
    public TimeMachine GetTimeMachine(Race race);
}
