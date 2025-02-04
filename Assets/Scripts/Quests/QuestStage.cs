using System.Linq;
using UnityEngine;

public class QuestStage
{
    private GameObject[] questStepPrefabs;
    
    private string _questId;
    private int _stageId;
    
    public QuestStepStatus[] QuestStepsData;

    public QuestStage(string questId, int stageId, GameObject[] questStepPrefabs)
    {
        _questId = questId;
        _stageId = stageId;
        QuestStepsData = new QuestStepStatus[questStepPrefabs.Length];
        this.questStepPrefabs = questStepPrefabs;
    }
    

    public void InitializeSteps()
    {
        for (int i = 0; i < questStepPrefabs.Length; i++)
        {
            QuestStep questStep = Object.Instantiate(questStepPrefabs[i], new Vector3(999f,0f,0f), Quaternion.identity, QuestManager.Instance.QuestContainer).GetComponent<QuestStep>();
            questStep.Init(_questId, _stageId, i);
            QuestStepsData[i] = new QuestStepStatus(QuestStepState.Active, questStep.Race);
        }
    }

    public bool AllStepsComplete()
    {
        return QuestStepsData.All(q => q.State == QuestStepState.Finished);
    }

    public string GetStageStatusText()
    {
        string status = "";
        foreach (QuestStepStatus step in QuestStepsData)
        {
            if (step.Race == Race.Both || step.Race == Core.Instance.MyRace())
            {
                switch (step.State)
                {
                    case QuestStepState.Active:
                        status += step.Status + "\n";
                        break;
                    case QuestStepState.Finished:
                        status += "<s>" + step.Status + "</s>\n";
                        break;
                }
            }
        }
        return status;
    }
}