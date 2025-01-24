using System.Collections.Generic;
using UnityEngine;

public class WallTile : ECSEntity, IInteractable, IDamageable {
    [SerializeField]
    private Sprite _icon;

    private Interactable _interactable;
   

    protected override void Awake() {
        base.Awake();
      
        _interactable = GetEcsComponent<Interactable>();
        _interactable.GetInfoFunc = GetInfoData;
        _interactable.OnCommandPerformed += OnCommandPerformed;
        Damageable damageable = GetEcsComponent<Damageable>();
        damageable.OnDiedAction += OnDiedAction;
    }

    private void OnDiedAction() {
        Vector2Int pos = GetEcsComponent<Gridable>().GetBottomLeftOnGrid;
        GridManager.Instance.RemoveWall(pos);
    }

    private void OnCommandPerformed(Command obj) {
        switch (obj) {
            case Command.Break:
                int fakeDamage = 1;
                GetEcsComponent<Damageable>().OnAttacked(fakeDamage);
                OnAttacked(1);
                break;
        }
    }

    protected virtual void OnAttacked(int damageAmount) { }

    protected virtual InfoBookData GetInfoData() {
        var d = new InfoBookData() {
            Icon = _icon,
            Name = gameObject.name,
            Resources = new List<ResorceData>()
        };
        return d;
    }
}