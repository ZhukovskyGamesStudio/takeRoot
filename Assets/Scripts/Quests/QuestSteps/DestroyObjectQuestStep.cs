using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectQuestStep : QuestStep
{
    [SerializeField] private List<string> _requiredObjectsIds;

    private void Start()
    {
        Core.GameEventsManager.WorldObjectsEvents.onDestroyed += RequiredObjectsDestroyed;
        UpdateQuestStepStatus(_status);
    }

    private void RequiredObjectsDestroyed(string id)
    {
        if (_requiredObjectsIds.Contains(id))
        {
            FinishStep();
        }
    }
}