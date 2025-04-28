using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PowerConsumer : ECSComponent
{
    public Vector2Int PowerSocketPosition;
    public GameObject Plug;
    public GameObject PowerSocket;
    public bool Connected;
    public Wire Wire;

    public void SetConnections(bool connected, Wire wire)
    {
        Connected = connected;
        Wire = wire;
        Plug.SetActive(connected);
    }
    public override int GetDependancyPriority()
    {
        return 0;
    }

    public override void Init(ECSEntity entity)
    {
        PowerSocketPosition = PowerSocket.transform.position.ToVector2Int();
    }
}