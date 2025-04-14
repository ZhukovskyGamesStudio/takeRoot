using UnityEngine;
using UnityEngine.Serialization;

public abstract class QuestStep : MonoBehaviour
{
    public Race Race;

    private string _questId;
    private int _stageId;
    private int _questStepId;
    [SerializeField] protected string _status;

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
        
        Core.QuestManager.UpdateQuestStepState(_questId, _stageId, _questStepId, QuestStepState.Finished);
        Core.QuestManager.AdvanceQuest(_questId, _stageId);
        Destroy(gameObject);
        Debug.Log($"{_stageId} step is finished");
    }

    protected void UpdateQuestStepStatus(string status)
    {
        Core.QuestManager.UpdateQuestStepStatus(_questId, _stageId, _questStepId, status);
    }
}