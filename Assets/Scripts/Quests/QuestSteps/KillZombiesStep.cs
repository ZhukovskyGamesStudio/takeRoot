using UnityEngine;

public class KillZombiesStep : QuestStep
{
    [SerializeField]private string _zombieId;
    private void Start()
    {
        var zombies = FindObjectsByType<Zombie>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var zombie in zombies)
        {
            //zombie.ChangeState(EnemyState.Passive);
        }
        Core.GameEventsManager.WorldObjectsEvents.onDied += RequiredZombieDied;
        UpdateQuestStepStatus(_status);
    }
    private void RequiredZombieDied(string id)
    {
        if (_zombieId == id)
        {
            FinishStep();
        }
    }
}