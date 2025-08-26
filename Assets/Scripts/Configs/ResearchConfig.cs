using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResearchConfig", menuName = "Scriptable Objects/ResearchConfig")]
public class ResearchConfig : ScriptableObject {
    public List<ResearchData> Researches;
}

[Serializable]
public class ResearchData {
    public string DisplayName;
    public Research Id;
    public int Price;
    public List<Sprite> Rewards;
    public List<Research> Requirements;
}

public enum Research {
    None,
    HomemadeInstruments,
    MaterialsProcessing
}
