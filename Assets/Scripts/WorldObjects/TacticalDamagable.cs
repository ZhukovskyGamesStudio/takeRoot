using System;
using System.Collections.Generic;
using UnityEngine;
using WorldObjects;

public class TacticalDamagable : ECSComponent {
    [field: SerializeField]
    public int Health { get; private set; }

    private TacticalInteractable _interactable;
    private Gridable _gridable;

    [SerializeField]
    protected List<ResourceData> _dropOnDestroyed;

    private Animatable _animatable;
    public Action  OnDiedAction;

    public override int GetDependancyPriority() {
        return 2;
    }

    public override void Init(ECSEntity entity) {
        if (entity is ITacticalInteractable) {
            entity.GetEcsComponent<TacticalInteractable>().AddToPossibleCommands(TacticalCommand.TacticalAttack);
            _interactable = entity.GetEcsComponent<TacticalInteractable>();
        }

        _animatable = entity.GetEcsComponent<Animatable>();
        _gridable = entity.GetEcsComponent<Gridable>();
    }

    public void OnAttacked(int damageAmount) {
        //_interactable.CancelCommand();
        Health -= damageAmount;
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
        _interactable.OnDestroyed();
        OnDiedAction?.Invoke();
        if (this != null) {
            Destroy(gameObject);
        }
    }
}