using UnityEngine;

public class KillZombiesStep : QuestStep
{
    private string _simpleZombieId;
    private void Start()
    {
        var zombies = FindObjectsByType<Zombie>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var zombie in zombies)
        {
            zombie.ChangeState(EnemyState.Passive);
        }
        GameEventsManager.Instance.WorldObjectsEvents.onDied += RequiredZombieDied;
        UpdateQuestStepStatus(_status);
    }
    private void RequiredZombieDied(string id)
    {
        if (_simpleZombieId.Contains(id))
        {
            FinishStep();
        }
    }
}