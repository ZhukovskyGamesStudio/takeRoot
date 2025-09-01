using UnityEngine;

public class TimeScaleService : ITimeScaleService {
    private TimeScaleConfig _config;

    private bool _canPause;
    private TimeMachine _timeMachine;
    
    public TimeScaleService(IConfigsProvider configProvider) {
        _config = configProvider.TimeScaleConfig;
    }

    private void TryPause() {
        if (!_canPause) return;
        
        _canPause = false;
        Time.timeScale = 0;
        _timeMachine.Use();
    }

    public void SetTimeMachine(TimeMachine timeMachine) {
        if (_timeMachine != null) {
            Debug.LogError("Time machine is already set");
            return;
        }

        _timeMachine = timeMachine;
    }

    public TimeMachine GetTimeMachine(Race race) {
        if (_timeMachine == null) return null;
        if (_timeMachine.Chargers.ContainsKey(race) && _timeMachine.Chargers[race] == null) return _timeMachine;
        return null;
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
