using System.Collections.Generic;
using UnityEngine;
using WorldObjects;

namespace Settlers
{
    public class TestEnemy : ECSEntity, ITacticalInteractable
    {
        [SerializeField]protected SpriteRenderer _icon;
        protected override void Awake()
        {
            base.Awake();
            TacticalInteractable interactable = GetEcsComponent<TacticalInteractable>();
            interactable.GetInfoFunc = GetInfoData;
            interactable.OnCommandPerformed += OnCommadPerformed;
        }

        private void OnCommadPerformed(TacticalCommand obj)
        {
            switch (obj)
            {
                case TacticalCommand.TacticalAttack:
                    int fakeDamage = 1;
                    GetEcsComponent<TacticalDamagable>().OnAttacked(fakeDamage);
                    OnAttacked(1);
                    break;
            }
        }

        protected virtual void OnAttacked(int damageAmount) { }
        
        protected virtual InfoBookData GetInfoData() {
            var d = new InfoBookData() {
                Icon = _icon.sprite,
                Name = gameObject.name,
                Resources = new List<ResourceData>()
            };
            return d;
        }
    }
}