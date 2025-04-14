using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestsView : MonoBehaviour, IHasRaceVariant
{
    [SerializeField] private Transform _questContainer;
    [SerializeField] private GameObject _questNotePrefab;
    [SerializeField] private Sprite _mainQuestIcon;
    [SerializeField] private Sprite _sideQuestIcon;
    private Dictionary<string, QuestNote> _questsNotes = new Dictionary<string, QuestNote>();

    public void RedrawQuest(Quest quest)
    {
        if (!_questsNotes.ContainsKey(quest.ID))
            AddQuestNote(quest);
        var questNote = _questsNotes[quest.ID];
        questNote.QuestName.text = quest.GetQuestName();
        questNote.QuestStatus.text = quest.GetQuestStatusText();
        questNote.QuestIcon.sprite = quest.config.IsMain ? _mainQuestIcon : _sideQuestIcon;
    }

    private void AddQuestNote(Quest quest)
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

    public IEnumerator FadeAway(Quest quest, float duration)
    {
        var questNote = _questsNotes[quest.ID];
        float currentTime = 0f;
        var name = questNote.QuestName;
        var status = questNote.QuestStatus;
        var icon = questNote.QuestIcon;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            name.color = new Color(name.color.r, name.color.g, name.color.b, alpha);
            status.color = new Color(status.color.r, status.color.g, status.color.b, alpha);
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    public void SetVariant(Race race)
    {
        if(Core.QuestManager != null)
            Core.QuestManager.RedrawAllQuests();
    }
}