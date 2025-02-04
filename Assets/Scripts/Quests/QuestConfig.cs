using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "QuestConfig", menuName = "Quests/QuestConfig")]
public class QuestConfig : ScriptableObject
{
    public string ID;
    public string Name;
    public Race Race;
    public QuestState State;
    public string[] NextQuests;
    public QuestStageConfig[] QuestStages;
    public QuestConfig[] QuestPrerequisites;
    public bool IsMain;
}
 