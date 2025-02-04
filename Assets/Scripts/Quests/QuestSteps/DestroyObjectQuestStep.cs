using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectQuestStep : QuestStep
{
    [SerializeField] private List<string> _requiredObjectsIds;

    private void Start()
    {
        GameEventsManager.Instance.WorldObjectsEvents.onDestroyed += RequeredObjectsDestroyed;
        UpdateQuestStepStatus(_status);
    }

    private void RequeredObjectsDestroyed(string id)
    {
        if (_requiredObjectsIds.Contains(id))
        {
            FinishStep();
        }
    }
}