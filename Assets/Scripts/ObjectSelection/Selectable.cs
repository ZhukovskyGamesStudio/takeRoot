using System;
using UnityEngine;

public abstract class Selectable : MonoBehaviour {
    [HideInInspector]
    public bool Selected;

    public Action OnSelect;

    public abstract object GetData(Race race);
}