using System;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : ECSComponent
{
    public Action OnDied;
    
    [field: SerializeField]
    public int Health { get; private set; }
    
    [SerializeField]
    protected List<ResourceData> _dropOnDestroyed;
    [SerializeField]
    private string _id;

    public void OnAttacked(int damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0)
        {
            OnDied?.Invoke();
            GameEventsManager.Instance.WorldObjectsEvents.OnDied(_id);
        }
    }
    
    public override int GetDependancyPriority()
    {
        return 0;
    }

    public override void Init(ECSEntity entity)
    { }
}