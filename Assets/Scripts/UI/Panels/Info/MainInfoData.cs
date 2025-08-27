using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
//TODO convert info scriptable object
public class MainInfoData {
    public Sprite Icon;
    public string Name;
    public string Description;

    //TODO should't be here
    public List<ResourceData> Resources = new List<ResourceData>();
}