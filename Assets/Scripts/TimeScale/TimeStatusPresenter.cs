public class TimeStatusPresenter {
    private ITimeScaleService _service;
    
    public TimeStatusPresenter(TimeStatusUI view, ITimeScaleService service) {
        _service = service;

        view.SetData(SetGameSpeed);
    }

    private void SetGameSpeed(GameSpeedType speed) {
        _service.SetTimeScale(speed);
    }
}
