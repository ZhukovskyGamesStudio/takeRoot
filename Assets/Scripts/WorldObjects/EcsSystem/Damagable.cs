using System;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : ECSComponent
{
    public Action OnDiedAction;
    
    private Gridable _gridable;
    
    [field: SerializeField]
    public int Health { get; private set; }
    
    [SerializeField]
    protected List<ResourceData> _dropOnDestroyed;
    [SerializeField]
    private string _id;
    

    public override void Init(ECSEntity entity)
    {
        _gridable = entity.GetEcsComponent<Gridable>();
    }

    public void OnAttacked(int damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0)
        {
            OnDiedAction?.Invoke();
            OnDied();
            Core.GameEventsManager.WorldObjectsEvents.OnDied(_id);
        }
    }

    private void OnDied()
    {
        Vector2Int pos = _gridable.GetBottomLeftOnGrid;
        ResourceManager.SpawnResourcesAround(_dropOnDestroyed, pos);
        if (this != null) {
            Destroy(gameObject);
        }
    }

    public override int GetDependancyPriority()
    {
        return 0;
    }
}