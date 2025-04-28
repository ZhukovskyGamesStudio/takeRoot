using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowerLight : ECSComponent
{
    private Light2D _light;
    private PowerConsumer _powerConsumer;

    public override int GetDependancyPriority()
    {
        return 0;
    }

    public override void Init(ECSEntity entity)
    {
        _light = entity.GetComponentInChildren<Light2D>();
        _powerConsumer = entity.GetComponent<PowerConsumer>();
    }

    private void Update()
    {
        _light.gameObject.SetActive(_powerConsumer.Connected);
    }
}