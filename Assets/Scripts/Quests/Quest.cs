using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestConfig config;

    public string ID;
    public QuestState State;
    public string[] NextQuests;
    
    public int CurrentStageIndex;
    public QuestStage[] Stages;

    public Quest(QuestConfig config)
    {
        this.config = config;
        ID = config.ID;
        State = config.State;
        NextQuests = config.NextQuests;
        Stages = new QuestStage[config.QuestStages.Length];
        CurrentStageIndex = 0;
    }
    
    public void InitializeStage()
    {
        var stageConfig = GetCurrentStageConfig();
        var stage = new QuestStage(ID, CurrentStageIndex, stageConfig.QuestStepPrefabs);
        Stages[CurrentStageIndex] = stage;
        stage.InitializeStage();
    }
    private QuestStageConfig GetCurrentStageConfig()
    {
        if (HasCurrentStage())
            return config.QuestStages[CurrentStageIndex];
        Debug.LogError("No quest step prefab found");
        return null;
    }
    
    public void MoveToNextStage()
    {
        CurrentStageIndex++;
    }
    public bool HasCurrentStage()
    {
        return CurrentStageIndex < config.QuestStages.Length;
    }

    //public QuestData GetQuestData()
    //{
    //    return new QuestData(config.ID, State, QuestStepData, config.Race, config.IsMain);
    //}

    public string GetQuestStatusText()
    {
        string status = "";
        foreach (QuestStage stage in Stages)
        {
            status += stage.GetStageStatusText();
        }
        return status;
    }
}