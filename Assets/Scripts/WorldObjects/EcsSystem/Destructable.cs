using System;
using System.Collections.Generic;
using UnityEngine;
using WorldObjects;

public class Destructable : ECSComponent {
    [field: SerializeField]
    public int Health { get; private set; }

    [SerializeField]
    protected List<ResourceData> _dropOnDestroyed;
    [SerializeField]
    private string _id;

    private Animatable _animatable;
    private Gridable _gridable;
    private Noisable _noisable;

    private Interactable _interactable;
    public Action OnDiedAction;

    public override int GetDependancyPriority() {
        return 2;
    }

    public override void Init(ECSEntity entity) {
        if (entity is IInteractable) {
            entity.GetEcsComponent<Interactable>().AddToPossibleCommands(Command.Break);
            _interactable = entity.GetEcsComponent<Interactable>();
        }
        
        _noisable = entity.GetEcsComponent<Noisable>();
        _animatable = entity.GetEcsComponent<Animatable>();
        _gridable = entity.GetEcsComponent<Gridable>();
    }

    public void OnAttacked(int damageAmount) {
        //_interactable.CancelCommand();
        Health -= damageAmount;
        _noisable?.MakeNoise();
        if (Health > 0) {
            _animatable?.TriggerDamaged();
        } else {
            OnDied();
            _interactable.CancelCommand();
            _animatable?.TriggerDied();
        }
    }

    private void OnDied() {
        Vector2Int pos = _gridable.GetBottomLeftOnGrid;
        ResourceManager.SpawnResourcesAround(_dropOnDestroyed, pos);
        GameEventsManager.Instance.WorldObjectsEvents.OnDestroyed(_id);
        _interactable.OnDestroyed();
        OnDiedAction?.Invoke();
        if (this != null) {
            Destroy(gameObject);
        }
    }
}