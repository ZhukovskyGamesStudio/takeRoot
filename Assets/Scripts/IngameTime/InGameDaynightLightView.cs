using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InGameDaynightLightView : NetworkBehaviour {
    [SerializeField]
    private Light2D _daynightLight;

    private IIngameTimeService _service;
    private IngameTimeConfig _config;

    public void Init(IngameTimeConfig config, IIngameTimeService service) {
        _config = config;
        _service = service;

        if (IsHost) {
            _service.IngameTimeData.TimeOfDayInSeconds.Subscribe(SetData);
        }
    }

    private void SetData(float timeOfDayInSeconds) {
        _daynightLight.color = _config.DaynightLightColorGradient.Evaluate(timeOfDayInSeconds / (_config.IngameDayInMinutes * 60));
        SetDataClientRpc(timeOfDayInSeconds);
    }

    [ClientRpc]
    private void SetDataClientRpc(float timeOfDayInSeconds) {
        _daynightLight.color = _config.DaynightLightColorGradient.Evaluate(timeOfDayInSeconds / (_config.IngameDayInMinutes * 60));
    }
}