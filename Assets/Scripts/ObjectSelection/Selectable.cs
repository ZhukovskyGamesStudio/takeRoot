using System;
using UnityEngine;

public abstract class Selectable : MonoBehaviour {
    public bool Selected;
    public Action OnSelect;

    public abstract object GetData();
}