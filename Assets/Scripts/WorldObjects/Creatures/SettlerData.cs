using System;
using System.Collections.Generic;
using UnityEngine;
using WorldObjects;

[Obsolete]
public class SettlerData : ECSComponent {
    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public Sprite InfoBookIcon;

    [field: SerializeField]
    public Mood _mood;

    [field: SerializeField]
    public Mode _mode;

    public Race Race;

    public float RoundAttackCooldown;

    public int Hp;
    public int MaxHp;
    public int Stress;
    public int MaxStress;

    public List<EquipmentType> PossibleEquipment = new();

    [HideInInspector]
    public List<Command> AvailableCommands = new();

    [SerializeField]
    private List<Command> CanPerformCommands = new();

    public Dictionary<EquipmentType, Command> EquipmentAvailableCommands = new();
    public Dictionary<EquipmentType, ResourceType> Equipped = new();

    public Vector2Int GetCellOnGrid => new(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

    private void Update() {
        if (RoundAttackCooldown > 0) {
            RoundAttackCooldown -= Time.deltaTime;
        }
    }

    public override int GetDependancyPriority() {
        return 0;
    }

    public override void Init(ECSEntity entity) {
        TacticalInteractable interactable = entity.GetEcsComponent<TacticalInteractable>();
        if (interactable != null) {
            interactable.GetInfoFunc = GetInfoData;
        }

        UpdateAvailableCommands();
    }

    private void UpdateAvailableCommands() {
        AvailableCommands.Clear();
        AvailableCommands.AddRange(CanPerformCommands);
        AvailableCommands.AddRange(EquipmentAvailableCommands.Values);
    }

    public void Equip(ResourceData resource) {
        EquipmentType equppedType = ResourcesHelper.GetEquipmentByResourceType(resource.ResourceType);
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
        switch (resource.ResourceType) {
            case ResourceType.Hammer:
                EquipmentAvailableCommands[equppedType] = Command.Break;
                break;
        }

        UpdateAvailableCommands();
    }

    public void Unequip(EquipmentType equipmentType) {
        ResourceType resType = Equipped[equipmentType];
        ResourceData resData = new() {
            Amount = 1,
            ResourceType = resType
        };
        Equipped.Remove(equipmentType);
        EquipmentAvailableCommands[equipmentType] = Command.None;
        UpdateAvailableCommands();

        ResourceManager.SpawnResourcesAround(new List<ResourceData> { resData }, GetCellOnGrid);
    }

    private InfoPanelData GetInfoData() {
        InfoPanelData data = new() {
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