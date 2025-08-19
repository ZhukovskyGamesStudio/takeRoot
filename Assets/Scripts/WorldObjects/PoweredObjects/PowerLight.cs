using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowerLight : ECSComponent
{
    [SerializeField]private SpriteRenderer _spriteRenderer;
    [SerializeField]private Light2D _light;
    [SerializeField]private Sprite _onSprite;
    [SerializeField]private Sprite _offSprite;
    private PowerConsumer _powerConsumer;

    public override int GetDependancyPriority()
    {
        return 0;
    }

    public override void Init(ECSEntity entity)
    {
        _powerConsumer = entity.GetComponent<PowerConsumer>();
    }

    private void Update()
    {
        _light.gameObject.SetActive(_powerConsumer.Connected);
        _spriteRenderer.sprite = _powerConsumer.Connected ? _onSprite : _offSprite;
    }
}