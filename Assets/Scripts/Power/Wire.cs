using UnityEngine;

public class Wire : ECSComponent
{
    public bool Connected;
    public PowerProvider Generator;
    public byte WireDirection = 0b0000;

    public void SetWire(bool connected, PowerProvider generator, byte wireDirection)
    {
        WireDirection = wireDirection;
        Connected = connected;
        Generator = generator;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Core.PowerManager.WireDirectionSprites[WireDirection];
        //spriteRenderer.color = Connected ? Color.red : Color.blue;
    }
    
    
    public override int GetDependancyPriority()
    {
        return 0;
    }

    public override void Init(ECSEntity entity)
    {
    }
    
}