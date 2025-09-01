public class TimeStatusPresenter {
    private ITimeScaleService _service;
    private readonly IIngameTimeService _ingameTimeService;

    public TimeStatusPresenter(TimeStatusUI view, ITimeScaleService service, IIngameTimeService ingameTimeService) {
        _service = service;
        _ingameTimeService = ingameTimeService;

        view.SetData(_ingameTimeService.IngameTimeData, SetGameSpeed, service.IsReadyForPause );
    }

    private void SetGameSpeed(GameSpeedType speed) {
        _service.SetTimeScale(speed);
    }
}
