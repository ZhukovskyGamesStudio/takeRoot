using System;
using UnityEngine;
using WorldObjects;

public class TacticalRoundAttack : ECSComponent
{
    private TacticalInteractable _tacticalInteractable;

    public float AttackCooldown {get; private set;}

    private void Update()
    {
        AttackCooldown -= Time.deltaTime;
    }
    public override int GetDependancyPriority()
    {
        return 0;
    }
    
    public override void Init(ECSEntity entity)
    {
        _tacticalInteractable = entity.GetEcsComponent<TacticalInteractable>();
        _tacticalInteractable.AddToPossibleCommands(TacticalCommand.RoundAttack);
        _tacticalInteractable.OnCommandPerformed += OnCommandPerformed;
    }

    private void OnCommandPerformed(TacticalCommand obj)
    {
        if (obj == TacticalCommand.RoundAttack)
        {
            DoRoundAttack();
        }
    }

    private void DoRoundAttack()
    {
        _tacticalInteractable.CommandToExecute.Settler.SettlerData.RoundAttackCooldown = Core.ConfigManager.CreaturesParametersConfig.RoundAttackCooldown;
        var attackCells = _tacticalInteractable.CommandToExecute.TacticalInteractable.Gridable.InteractableCells;
        var zombies = FindObjectsByType<Zombie>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (Zombie zombie in zombies)
        foreach (Vector2Int position in zombie.Gridable.GetOccupiedPositions())
        {
            if (attackCells.Contains(position))
            {
                zombie.GetEcsComponent<Damagable>().OnAttacked(1);
                break;
            }
        }
        _tacticalInteractable.CommandToExecute?.TriggerCancel?.Invoke();
        _tacticalInteractable.CancelCommand();
    }
}