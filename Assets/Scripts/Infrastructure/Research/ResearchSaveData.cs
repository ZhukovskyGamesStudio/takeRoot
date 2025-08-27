using System;
using System.Collections.Generic;

[Serializable]
public class ResearchSaveData {
    public Dictionary<Research, int> ResearchProgress = new();
    public Research CurrentResearch;

    public ResearchSaveData() {
        foreach (Research research in Enum.GetValues(typeof(Research))) {
            ResearchProgress[research] = 0;
        }
    }
}