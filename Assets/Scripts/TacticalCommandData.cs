using System;
using UnityEngine;
using WorldObjects;

[Serializable]
public class TacticalCommandData
{
    public TacticalCommand TacticalCommandType;
    public Vector2Int TargetPosition;
    public TacticalInteractable TacticalInteractable;
    public Settler Settler;
    public PlannedCommandView PlannedCommandView;
    public Action TriggerCancel;
}