using UnityEngine;

public class PowerProvider : ECSComponent
{
    public Vector2Int PowerSocketPosition;
    public GameObject Plug;
    public GameObject PowerSocket;
    public void SetConnections(bool connected)
    {
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