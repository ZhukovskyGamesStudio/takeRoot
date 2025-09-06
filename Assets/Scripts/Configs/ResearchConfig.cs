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
    [TextArea]
    public string Description;
    public Research Id;
    public int Price;
    public List<SpriteAndName> Rewards;
    public List<Research> Requirements;

    public bool IsDisabled;
    
    [HideInInspector]
    public bool Researchable;
}

[Serializable]
public class SpriteAndName {
    public string Name;
    public Sprite Sprite;
}

public enum Research {
    None,
    HomemadeInstruments,
    MaterialsProcessing,
    AdvancedResearches,
    ArtifactsDetector,
    TimeStopper,
    HomemadeSanitary,
    TubeCooler,
    TubeDistiller,
    Waterers,
    HouseLamp,
    RestPlace,
    SettlersCare,
    Cultivation,
    ChemicalProcessing,
    BioFuel,
    BioGenerator,
    Lubricant,
    Fertilizers,
    RestPlace2Flowers,
    RestPlace2Robots,
    SettlersCare2
}