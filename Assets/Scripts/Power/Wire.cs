using UnityEngine;

public class Wire : ECSComponent
{
    public bool Connected;
    public PowerProvider Generator;
    public byte WireDirection = 0b0000;
    
    private SpriteRenderer _spriteRenderer;

    public void SetWire(bool connected, PowerProvider generator, byte wireDirection)
    {
        WireDirection = wireDirection;
        Connected = connected;
        Generator = generator;
        _spriteRenderer.sprite = Core.PowerManager.WireDirectionSprites[WireDirection];
        //spriteRenderer.color = Connected ? Color.red : Color.blue;
    }
    public override int GetDependancyPriority()
    {
        return 0;
    }

    public override void Init(ECSEntity entity)
    {
        _spriteRenderer  = entity.GetComponent<SpriteRenderer>();
    }
    
}