using UnityEngine;
using UnityEngine.Serialization;

public abstract class QuestStep : MonoBehaviour
{
    public Race Race;

    private string _questId;
    private int _stageId;
    private int _questStepId;

    public bool IsFinished;
    public void Init(string questId,int stageId, int stepIndex)
    {
        _questId = questId;
        _stageId = stageId;
        _questStepId = stepIndex;
    }
    
    protected void FinishStep()
    {
        if (IsFinished) return;
        IsFinished = true;
        
        QuestManager.Instance.UpdateQuestStepStatus(_questId, _stageId, _questStepId, QuestStepState.Finished);
        QuestManager.Instance.AdvanceQuest(_questId, _stageId);
        Destroy(gameObject);
        Debug.Log($"{_stageId} step is finished");
    }

    protected void UpdateQuestStepData(QuestStepStatus stepStatus)
    {
        QuestManager.Instance.UpdateQuestStepStatus(_questId, _stageId, _questStepId, stepStatus);
    }
}