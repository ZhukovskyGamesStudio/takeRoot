using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfigsProvider : IConfigsProvider {
    public List<FarmingPlantConfig> FarmingConfigs { get; set; }

    public ConfigsProvider() {
        CacheConfigs();
    }

    private void CacheConfigs() {
        FarmingConfigs = Resources.LoadAll<FarmingPlantConfig>("Configs/FarmingPlants").ToList();
    }
}