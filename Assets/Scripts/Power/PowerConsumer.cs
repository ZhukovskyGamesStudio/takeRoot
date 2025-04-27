using System;
using UnityEngine;

public class PowerConsumer : ECSComponent
{
    public Vector2Int PlugPosition;
    public bool Connected;
    public Wire Wire;

    public void SetConnections(bool connected, Wire wire)
    {
        Connected = connected;
        Wire = wire;
    }
    public override int GetDependancyPriority()
    {
        return 0;
    }

    public override void Init(ECSEntity entity)
    {
        PlugPosition = transform.position.ToVector2Int();
    }
}