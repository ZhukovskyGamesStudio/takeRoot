using System;
using UnityEngine;

[Serializable]
public class CommandTargetData {
    [HideInInspector]
    public int Id;

    [Header("Jobs")]
    
    [HideInInspector]
    public JobType JobCapabilities;

    [HideInInspector]
    public JobType CurrentJob;
    
    [HideInInspector]
    public AI.Settler AssignedSettler;
    public bool HasJob => CurrentJob != JobType.None;
    public bool Reserved => AssignedSettler != null;

    public MainInfoData MainInfoData;
}