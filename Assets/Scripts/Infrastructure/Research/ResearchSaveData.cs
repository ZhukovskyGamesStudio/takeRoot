using System;
using System.Collections.Generic;

[Serializable]
public class ResearchSaveData {
    public Dictionary<Research, int> ResearchProgress;
    public Research CurrentResearch;
}
