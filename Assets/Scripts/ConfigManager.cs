using System;
using UnityEngine;

public class ConfigManager : MonoBehaviour {
    public static ConfigManager Instance;

    [field: SerializeField]
    public CreaturesParametersConfig CreaturesParametersConfig { get; private set; }

    private void Awake() {
        Instance = this;
    }
}