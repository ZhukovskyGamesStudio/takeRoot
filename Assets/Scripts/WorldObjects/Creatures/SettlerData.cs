using System;
using System.Collections.Generic;
using UnityEngine;
using WorldObjects;

public class SettlerData : ECSComponent {
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public Sprite InfoBookIcon;

    [field: SerializeField]
    public Mood _mood;

    [field: SerializeField]
    public Mode _mode;

    public Race Race;
    
    public float RoundAttackCooldown;
    
    public List<EquipmentType> PossibleEquipment = new List<EquipmentType>();
    public Dictionary<EquipmentType, ResourceType> Equipped = new Dictionary<EquipmentType, ResourceType>();

    public Vector2Int GetCellOnGrid => new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

    private void Update()
    {
        if (RoundAttackCooldown > 0)
            RoundAttackCooldown -= Time.deltaTime;
    }

    public override int GetDependancyPriority() {
        return 0;
    }

    public override void Init(ECSEntity entity)
    {
        var interactable = entity.GetEcsComponent<TacticalInteractable>();
        if (interactable != null)
            interactable.GetInfoFunc = GetInfoData;
    }

    public void Equip(ResourceData resource) {
        var equppedType = ResourcesHelper.GetEquipmentByResourceType(resource.ResourceType);
        if (equppedType == EquipmentType.None) {
            return;
        }

        if (Equipped.TryGetValue(equppedType, out ResourceType value)) {
            if (value == resource.ResourceType) {
                return;
            }

            Unequip(equppedType);
        }

        Equipped[equppedType] = resource.ResourceType;
    }

    public void Unequip(EquipmentType equipmentType) {
        ResourceType resType = Equipped[equipmentType];
        var resData = new ResourceData() {
            Amount = 1,
            ResourceType = resType
        };
        Equipped.Remove(equipmentType);
        ResourceManager.SpawnResourcesAround(new List<ResourceData>() { resData }, GetCellOnGrid);
    }

    private InfoBookData GetInfoData()
    {
        InfoBookData data = new InfoBookData()
        {
            Icon = InfoBookIcon,
            Name = Name,
            Resources = new List<ResourceData>()
        };
        return data;
    }
}

[Serializable]
public enum EquipmentType {
    None,
    Hand
}