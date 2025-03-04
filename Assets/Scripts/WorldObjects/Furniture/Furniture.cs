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

    private void OnCommandPerformed(CommandData obj) {
        switch (obj.CommandType) {
            case Command.Search:
                GetEcsComponent<Searchable>().OnExplored();
                OnExplored();
                break;
            case Command.Break:
                int fakeDamage = 1;
                GetEcsComponent<Destructable>().OnAttacked(fakeDamage);
                OnAttacked(1);
                break;
        }
    }

    protected virtual void OnExplored() { }
    protected virtual void OnAttacked(int damageAmount) { }

    protected virtual InfoBookData GetInfoData() {
        InfoBookData d = new InfoBookData() {
            Icon = _icon.sprite,
            Name = gameObject.name,
            Resources = new List<ResourceData>()
        };
        Storagable storagable = GetEcsComponent<Storagable>();
        if (storagable != null) {
            d.Resources.Add(storagable.Resource);
        }

        return d;
    }
}