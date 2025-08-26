using System.Collections.Generic;
using UnityEngine;

public class ResearchPanelView : MonoBehaviour {
    [SerializeField]
    private List<ResearchView> _researches;
    
    public void InitData(Dictionary<Research, ResearchData> researches) {
        foreach (ResearchView research in _researches) {
            research.InitData(researches[research.Id]);
        }
    }

    public void UpdateData(ResearchSaveData data) {
        foreach (ResearchView research in _researches) {
            research.UpdateData(data.ResearchProgress[research.Id], data.CurrentResearch == research.Id);
        }
    }
}
