using System;
using System.Collections.Generic;

public class QuestData
{
    public string ID;
    public QuestState State;
    public Race Race;
    public bool IsMain;
    public QuestStepStatus[] QuestStepData;

    public QuestData(string id, QuestState state, QuestStepStatus[] questStepData, Race race, bool isMain)
    {
        ID = id;
        State = state;
        Race = race;
        IsMain = isMain;
        QuestStepData = questStepData;
    }
}

[Serializable]
public enum QuestState
{
    Unavailable,
    Available,
    InProgress,
    CanComplete,
    Completed
}