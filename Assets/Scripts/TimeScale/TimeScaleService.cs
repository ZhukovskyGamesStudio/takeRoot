using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class TimeScaleService : ITimeScaleService {
    private TimeScaleConfig _config;

    private TimeMachine _timeMachine;

    private Dictionary<Race, GameSpeedType> _selectedSpeeds = new();

    public TimeScaleService(IConfigsProvider configProvider) {
        _config = configProvider.TimeScaleConfig;
        _selectedSpeeds[Race.Plants] = GameSpeedType.Normal;
        _selectedSpeeds[Race.Robots] = GameSpeedType.Normal;
    }

    private void Pause() {
        IsReadyForPause.Value = false;
        NetworkDataHolder.Instance.SetGameSpeedClientRpc(0);
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
        if (_timeMachine == null) {
            return null;
        }
        if (_timeMachine.Chargers.ContainsKey(race) && _timeMachine.Chargers[race] == null) {
            return _timeMachine;
        }
        return null;
    }

    public void SetTimeScaleOnly(GameSpeedType type, Race race) {
        _selectedSpeeds[race] = type;
    }

    public void SetTimeScale(GameSpeedType type, Race race) {
        Debug.Log($"Setting time scale {type} {race}");
        SetTimeScaleOnly(type, race);
        
        Debug.Log("Selected speeds: " + string.Join(", ", _selectedSpeeds.Select(kv => kv.Key + ": " + kv.Value)));
        
        GameSpeedType min = _selectedSpeeds.Values.OrderBy(v => (int)v).First();

        if (min == GameSpeedType.Paused && !IsReadyForPause.Value) {
            Debug.Log("Not ready to pause");
            return;
        }

        if (min == GameSpeedType.Paused) {
            Debug.Log("Ready to pause");
            Pause();
            return;
        }
        Debug.Log("Setting time scale to " + _config.TimeScales[min]);
        NetworkDataHolder.Instance.SetGameSpeedClientRpc(_config.TimeScales[min]);
        NetworkDataHolder.Instance.SetSelectedTimeScaleClientRpc(type,race);
    }

    public GameSpeedType GetTimeScale(Race race) {
        return  _selectedSpeeds[race];
    }

    public ReactiveProperty<bool> IsReadyForPause { get; set; } = new ReactiveProperty<bool>(false);

    public void SetReadyToPause() {
        IsReadyForPause.Value = true;
    }
}