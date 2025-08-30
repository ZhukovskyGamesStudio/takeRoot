using UnityEngine;

public class TimeScaleService : ITimeScaleService {
    private TimeScaleConfig _config;

    private bool _canPause;
    
    public TimeScaleService(TimeScaleConfig config) {
        _config = config;
    }

    private void TryPause() {
        if (!_canPause) return;
        
        _canPause = false;
        Time.timeScale = 0;
    }
    
    public void SetTimeScale(GameSpeedType type) {
        if (type == GameSpeedType.Paused) {
            TryPause();
            return;
        }

        Time.timeScale = _config.TimeScales[type];
    }

    public void SetReadyToPause() {
        _canPause = true;
    }
}
