using CodeBase.Services;
using UnityEngine;
using WorldObjects;

public class TacticalMergable : ECSComponent {
    private Race _race;
    private TacticalInteractable _tacticalInteractable;

    public override int GetDependancyPriority() {
        return 0;
    }

    public override void Init(ECSEntity entity) {
        _race = entity.GetEcsComponent<SettlerData>().Race;
        if (entity.GetEcsComponent<TacticalMergable>() == null) {
            return;
        }

        _tacticalInteractable = entity.GetEcsComponent<TacticalInteractable>();
        _tacticalInteractable.AddToPossibleCommands(TacticalCommand.Merge);
        _tacticalInteractable.OnCommandPerformed += OnCommandPerformed;
    }

    private void OnCommandPerformed(TacticalCommand obj) {
        if (obj == TacticalCommand.Merge) {
            MergeSettlers();
        }
    }

    private void MergeSettlers() {
        Vector3 spawnPos = _tacticalInteractable.CommandToExecute.TacticalInteractable.Gridable.GetCenterOnGrid;

        Settler settler = _tacticalInteractable.CommandToExecute.Settler;
        ObsoleteCoreEntryPoint.SettlersManager.DestroySettler(
            _tacticalInteractable.CommandToExecute.TacticalInteractable.GetComponent<Settler>());
        ObsoleteCoreEntryPoint.SettlersSelectionManager.TryUnselectSpecificSettler(settler);
        ObsoleteCoreEntryPoint.SettlersManager.DestroySettler(settler);

        ServiceLocator.Container.Single<IGameFactory>().CreateSettler("combined", new Vector3((int)spawnPos.x, (int)spawnPos.y));

        ObsoleteCoreEntryPoint.GameEventsManager.WorldObjectsEvents.OnSettlersMerged();
    }
}