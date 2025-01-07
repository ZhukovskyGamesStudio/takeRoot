using System.Collections.Generic;
using UnityEngine;

public class Searchable : ECSComponent {
    [field: SerializeField]
    public List<ResorceData> DropOnSearch { get; private set; }

    private Interactable _interactable;
    private Animatable _animatable;

    public override void Init(ECSEntity entity) {
        if (entity is IInteractable) {
            entity.GetEcsComponent<Interactable>().AddToPossibleCommands(Command.Search);
            _interactable = entity.GetEcsComponent<Interactable>();
        }

        _animatable = entity.GetEcsComponent<Animatable>();
    }

    public override int GetDependancyPriority() {
        return 1;
    }

    public void OnExplored() {
        _interactable.RemoveFromPossibleCommands(Command.Search);
        _interactable.CancelCommand();
        ResourceManager.SpawnResourcesAround(DropOnSearch, _interactable.GetInteractableSell);

        _animatable?.TriggerExplored();
    }
}