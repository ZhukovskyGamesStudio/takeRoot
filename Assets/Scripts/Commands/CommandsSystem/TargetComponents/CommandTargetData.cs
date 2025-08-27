using System;
using UnityEngine;

[Serializable]
public class CommandTargetData {
    public int Id;

    [Header("Jobs")]
    public JobType JobCapabilities;

    public JobType CurrentJob;
    public AI.Settler AssignedSettler;
    public bool HasJob => CurrentJob != JobType.None;
    public bool Reserved => AssignedSettler != null;

    public InfoPanelData InfoPanelData;
}