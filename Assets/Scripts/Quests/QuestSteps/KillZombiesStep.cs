using UnityEngine;

public class KillZombiesStep : QuestStep {
    [SerializeField]
    private string _zombieId;

    private void Start() {
        Zombie[] zombies = FindObjectsByType<Zombie>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (Zombie zombie in zombies) {
            //zombie.ChangeState(EnemyState.Passive);
        }

        ObsoleteCoreEntryPoint.GameEventsManager.WorldObjectsEvents.onDied += RequiredZombieDied;
        UpdateQuestStepStatus(_status);
    }

    private void RequiredZombieDied(string id) {
        if (_zombieId == id) {
            FinishStep();
        }
    }
}