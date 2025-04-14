using UnityEngine;
using WorldObjects;

public class TacticalMergable : ECSComponent
{
    private Race _race;
    private TacticalInteractable _tacticalInteractable;

    public override int GetDependancyPriority()
    {
        return 0;
    }
    public override void Init(ECSEntity entity)
    {
        _race = entity.GetEcsComponent<SettlerData>().Race;
        if (entity.GetEcsComponent<TacticalMergable>() == null)
            return;

        _tacticalInteractable = entity.GetEcsComponent<TacticalInteractable>();
        _tacticalInteractable.AddToPossibleCommands(TacticalCommand.Merge);
        _tacticalInteractable.OnCommandPerformed += OnCommandPerformed;
    }

    private void OnCommandPerformed(TacticalCommand obj)
    {
        if (obj == TacticalCommand.Merge)
        {
            MergeSettlers();
        }
    }
    private void MergeSettlers()
    {
        var spawnPos = _tacticalInteractable.CommandToExecute.TacticalInteractable.Gridable.GetCenterOnGrid;


        var settler = _tacticalInteractable.CommandToExecute.Settler;
        Core.SettlersManager.DestroySettler(
            _tacticalInteractable.CommandToExecute.TacticalInteractable.GetComponent<Settler>());
        Core.SettlersSelectionManager.TryUnselectSpecificSettler(settler);
        Core.SettlersManager.DestroySettler(settler);
        Core.SettlersManager.CreateCombinedSettlerAt(new Vector2Int((int)spawnPos.x, (int)spawnPos.y));
        Core.GameEventsManager.WorldObjectsEvents.OnSettlersMerged();
    }
}