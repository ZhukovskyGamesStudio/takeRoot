using System.Collections.Generic;
using UnityEngine;

public abstract class Furniture : ECSEntity, IInteractable, IDamageable {
    [SerializeField]
    protected SpriteRenderer _icon;

    protected override void Awake() {
        base.Awake();
        Interactable interactable = GetEcsComponent<Interactable>();
        interactable.GetInfoFunc = GetInfoData;
        interactable.OnCommandPerformed += OnCommandPerformed;
    }

    private void OnCommandPerformed(Command obj) {
        switch (obj) {
            case Command.Search:
                GetEcsComponent<Searchable>().OnExplored();
                OnExplored();
                break;
            case Command.Break:
                int fakeDamage = 1;
                GetEcsComponent<Damageable>().OnAttacked(fakeDamage);
                OnAttacked(1);
                break;
        }
    }

    protected virtual void OnExplored() { }
    protected virtual void OnAttacked(int damageAmount) { }

    protected virtual InfoBookData GetInfoData() {
        var d = new InfoBookData() {
            Icon = _icon.sprite,
            Name = gameObject.name,
            Resources = new List<ResorceData>()
        };
        return d;
    }
}