using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestsView : MonoBehaviour, IHasRaceVariant
{
    [SerializeField] private Transform _questContainer;
    [SerializeField] private GameObject _questNotePrefab;
    private Dictionary<string, QuestNote> _questsNotes = new Dictionary<string, QuestNote>();

    public void RedrawQuest(Quest quest)
    {
        if (!_questsNotes.ContainsKey(quest.ID))
            AddQuestNote(quest);
        var questNote = _questsNotes[quest.ID];
        questNote.questName.text = quest.config.Name;
        questNote.questStatus.text = quest.GetQuestStatusText();
    }

    public void AddQuestNote(Quest quest)
    {
        var questNote = Instantiate(_questNotePrefab, _questContainer).GetComponent<QuestNote>();
        _questsNotes.Add(quest.ID, questNote);
    }

    public void RemoveQuestNote(Quest quest)
    {
        var questNote = _questsNotes[quest.ID];
        _questsNotes.Remove(quest.ID);
        Destroy(questNote.gameObject);
    }

    public void ClearQuestNotes()
    {
        foreach (var key in _questsNotes.Keys.ToList())
        {
            var questNote = _questsNotes[key];
            _questsNotes.Remove(key);
            Destroy(questNote.gameObject);
        }
    }

    public void SetVariant(Race race)
    {
        if(QuestManager.Instance != null)
            QuestManager.Instance.RedrawAllQuests();
    }
}