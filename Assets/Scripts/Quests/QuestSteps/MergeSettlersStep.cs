public class MergeSettlersStep : QuestStep {
    private void Start() {
        UpdateQuestStepStatus(_status);
        ObsoleteCoreEntryPoint.GameEventsManager.WorldObjectsEvents.onSettlersMerged += OnSettlersMerged;
    }

    private void OnSettlersMerged() {
        FinishStep();
    }
}