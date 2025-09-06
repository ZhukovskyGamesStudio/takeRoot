using UniRx;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IngameTimeService : IIngameTimeService, IUpdatable {
    private readonly IUpdateService _updateService;
    private readonly InGameDaynightLightView _globalDaynightLight;
    private IngameTimeConfig _config;

    public IngameTimeService(IConfigsProvider configsProvider, IUpdateService updateService, InGameDaynightLightView globalDaynightLight) {
        _config = configsProvider.IngameTimeConfig;
        _updateService = updateService;
        _globalDaynightLight = globalDaynightLight;
       
    
        InitTimeData();
        
        _globalDaynightLight.Init(_config, this);
        
    }

    private void InitTimeData() {
        IngameTimeData = new IngameTimeData();
        IngameTimeData.Day.Value = 1;
        IngameTimeData.CurrentDayPartType.Value = DayPartType.Daytime;
        IngameTimeData.TimeOfDayInSeconds.Value = _config.IngameDayInMinutes * 60 * _config.DaytimeStartPercent;
        IngameTimeData.TimeLeftToChange.Value = _config.IngameDayInMinutes * 60 * _config.DaytimePercent;
    }

    public void Update() {
        IngameTimeData.TimeOfDayInSeconds.Value += Time.deltaTime;
        if(IngameTimeData.TimeOfDayInSeconds.Value >= _config.IngameDayInMinutes * 60) {
            IngameTimeData.TimeOfDayInSeconds.Value = 0;
            IngameTimeData.Day.Value++;
        }
        IngameTimeData.CurrentDayPartType.Value = IsNowDaytime ? DayPartType.Daytime : DayPartType.Nighttime;
        IngameTimeData.TimeLeftToChange.Value = SecondsLeftToChangeDayPart();
    }

    private bool IsNowDaytime => IngameTimeData.TimeOfDayInSeconds.Value >= DaytimeStartInSeconds &&
                                 IngameTimeData.TimeOfDayInSeconds.Value < DaytimeEndInSeconds;

    private float SecondsLeftToChangeDayPart() {
        if (IsNowDaytime) {
            return DaytimeEndInSeconds - IngameTimeData.TimeOfDayInSeconds.Value;
        }

        if (IngameTimeData.TimeOfDayInSeconds.Value < DaytimeStartInSeconds) {
            return DaytimeStartInSeconds - IngameTimeData.TimeOfDayInSeconds.Value;
        }

        return _config.IngameDayInMinutes * 60 - IngameTimeData.TimeOfDayInSeconds.Value + DaytimeStartInSeconds;
    }

    private float DaytimeEndInSeconds => (_config.DaytimeStartPercent + _config.DaytimePercent) * _config.IngameDayInMinutes * 60;

    private float DaytimeStartInSeconds => _config.DaytimeStartPercent * _config.IngameDayInMinutes * 60;

    public void Dispose() {
        _updateService.Unregister(this);
    }

    public IngameTimeData IngameTimeData { get; set; }
}