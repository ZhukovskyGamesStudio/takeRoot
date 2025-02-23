using System;
using UnityEngine;

public class ConfigManager : MonoBehaviour, IInitableInstance {
    [field: SerializeField]
    public CreaturesParametersConfig CreaturesParametersConfig { get; private set; }
    
    [field: SerializeField]
    public ZombieConfig ZombieConfig { get; private set; }
    
    public void Init() {
        Core.ConfigManager = this;
    }
}