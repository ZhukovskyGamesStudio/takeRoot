using System;
using UnityEngine;

public class Destructable : ECSComponent
{
    [SerializeField] private string _id;


    private void OnDestroy()
    {
        GameEventsManager.Instance.WorldObjectsEvents.OnDestroyed(_id);
    }

    public override int GetDependancyPriority()
    {
        return 0;
    }

    public override void Init(ECSEntity entity)
    { }
}