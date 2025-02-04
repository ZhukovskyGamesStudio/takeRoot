using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour, IInitableInstance
{
    public static QuestManager Instance;
    
    [SerializeField]private QuestsView _questsView;

    private Dictionary<string, Quest> _quests;
    public Transform QuestContainer => transform;

    public void Init()
    {
        Instance = this;
        _quests = CreateQuestsMap();
        
        StartQuest("door");
    }
    private Dictionary<string, Quest> CreateQuestsMap()
    {
        QuestConfig[] questConfigs = Resources.LoadAll<QuestConfig>("Quests");
        Debug.Log(questConfigs.Length);
        Dictionary<string, Quest> quests = new Dictionary<string, Quest>();
        foreach (QuestConfig config in questConfigs)
        {
            if (quests.ContainsKey(config.ID))
            {
                Debug.LogWarning("Quest " + config.ID + " already exists");
            }
            
            Quest quest = new Quest(config);
            quests.Add(config.ID, quest);
        }
        return quests;
    }

    private void StartQuest(string questId)
    {
        Quest quest = GetQuestById(questId);
        if (!CanTakeQuest(quest)) return;
        
        quest.State = QuestState.InProgress;
        quest.InitializeStage();
        if (quest.config.Race == Core.Instance.MyRace() || quest.config.Race == Race.Both)
            _questsView.AddQuestNote(quest);
    }
    
    public void AdvanceQuest(string questId, int stageId)
    {
        Quest quest = GetQuestById(questId);
        if (!quest.Stages[stageId].AllStepsComplete())
        {
            if (quest.config.Race == Core.Instance.MyRace() || quest.config.Race == Race.Both)
                _questsView.RedrawQuest(quest);
            return;
        }
        quest.MoveToNextStage();
        if (quest.HasCurrentStage())
        {
            quest.InitializeStage();
            if (quest.config.Race == Core.Instance.MyRace() || quest.config.Race == Race.Both)
                _questsView.RedrawQuest(quest);
        }
        else
            FinishQuest(questId);
    }

    public void FinishQuest(string questId)
    {
        Quest quest = GetQuestById(questId);
        quest.State = QuestState.Completed;
        foreach (string nextQuest in quest.NextQuests)
        {
            StartQuest(nextQuest);
        }
        if (quest.config.Race == Core.Instance.MyRace() || quest.config.Race == Race.Both)
            _questsView.RemoveQuestNote(quest);
    }

    public void UpdateQuestStepStatus(string questId, int stageId, int stepId, QuestStepStatus status)
    {
        Quest quest = GetQuestById(questId);
        quest.Stages[stageId].QuestStepsData[stepId] = status;
        _questsView.RedrawQuest(quest);
    }
    public void UpdateQuestStepStatus(string questId, int stageId, int stepId, QuestStepState state)
    {
        Quest quest = GetQuestById(questId);
        quest.Stages[stageId].QuestStepsData[stepId].State = state;
        _questsView.RedrawQuest(quest);
    }

    private bool CanTakeQuest(Quest quest)
    {
        foreach (QuestConfig questConfig in quest.config.QuestPrerequisites)
        {
            if (_quests[questConfig.ID].State != QuestState.Completed)
                return false;
        }
        return true;
    }

    public void RedrawAllQuests()
    {
        var quests = _quests.Values.Where(q => q.State == QuestState.InProgress &&
                                               (q.config.Race == Core.Instance.MyRace() || 
                                               q.config.Race == Race.Both));
        _questsView.ClearQuestNotes();
        foreach (Quest quest in quests)
        {
            _questsView.RedrawQuest(quest);
        }
    }
    private Quest GetQuestById(string questId)
    {
        var quest = _quests[questId];
        if (quest == null)
            Debug.LogWarning("Quest " + questId + " does not exist");
        return quest;
    }
    
}