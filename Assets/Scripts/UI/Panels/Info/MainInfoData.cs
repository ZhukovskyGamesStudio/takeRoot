using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MainInfoData {
    public Sprite Icon;
    public string Name;
    public string Description;

    public List<ResourceData> Resources = new List<ResourceData>();
}