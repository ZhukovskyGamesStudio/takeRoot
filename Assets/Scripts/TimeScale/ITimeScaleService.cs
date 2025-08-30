public interface ITimeScaleService : IService
{
    public void SetTimeScale(GameSpeedType type);
    public void SetReadyToPause();
}
