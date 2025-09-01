using UniRx;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InGameDaynightLightView : MonoBehaviour {
    [SerializeField]
    private Light2D _daynightLight;

    private IIngameTimeService _service;
    private IngameTimeConfig _config;

    public void Init(IngameTimeConfig config, IIngameTimeService service) {
        _config = config;
        _service = service;
        _service.IngameTimeData.TimeOfDayInSeconds.Subscribe(SetData);
    }

    public void SetData(float timeOfDayInSeconds) {
        _daynightLight.color = _config.DaynightLightColorGradient.Evaluate(timeOfDayInSeconds / (_config.IngameDayInMinutes * 60));
    }
}