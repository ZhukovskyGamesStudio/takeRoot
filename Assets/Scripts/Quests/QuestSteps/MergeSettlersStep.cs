using System;

public class MergeSettlersStep : QuestStep
{
    private void Start()
    {
        UpdateQuestStepStatus(_status);
        GameEventsManager.Instance.WorldObjectsEvents.onSettlersMerged += OnSettlersMerged;
    }

    private void OnSettlersMerged()
    {
        FinishStep();
    }
}