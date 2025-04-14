using System;

public class MergeSettlersStep : QuestStep
{
    private void Start()
    {
        UpdateQuestStepStatus(_status);
        Core.GameEventsManager.WorldObjectsEvents.onSettlersMerged += OnSettlersMerged;
    }

    private void OnSettlersMerged()
    {
        FinishStep();
    }
}