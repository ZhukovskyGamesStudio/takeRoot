using UnityEngine;
using WorldObjects;

public class TacticalEquippable : ECSComponent {
    private ResourceView _resourceView;
    private TacticalInteractable _tacticalInteractable;

    public override int GetDependancyPriority() {
        return 2;
    }

    public override void Init(ECSEntity entity) {
        _resourceView = entity as ResourceView;
        if (entity.GetEcsComponent<TacticalInteractable>() == null) {
            return;
        }

        _tacticalInteractable = entity.GetEcsComponent<TacticalInteractable>();
        _tacticalInteractable.AddToPossibleCommands(TacticalCommand.Equip);
        _tacticalInteractable.OnCommandPerformed += OnCommandPerformed;
    }

    private void OnCommandPerformed(TacticalCommand obj) {
        if (obj == TacticalCommand.Equip) {
            OnEquipped();
        }
    }

    private void OnEquipped() {
        _tacticalInteractable.CommandToExecute.Settler.Equip(_resourceView.ResourceData);
        _tacticalInteractable.OnDestroyed();
    }

    public EquipmentType GetEquipmentType() => ResourcesHelper.GetEquipmentByResourceType(_resourceView.ResourceType);
}