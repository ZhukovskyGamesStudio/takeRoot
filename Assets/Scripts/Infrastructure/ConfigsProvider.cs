using System.Collections.Generic;
using System.Linq;
using GameResources;
using UnityEngine;

public class ConfigsProvider : IConfigsProvider {
    public List<FarmingPlantConfig> FarmingConfigs { get; set; }
    public ResourcesTable ResourcesTable { get; set; }
    public ResourcesConfig ResourcesConfig { get; set; }

    public ConfigsProvider() {
        CacheConfigs();
    }

    private void CacheConfigs() {
        FarmingConfigs = Resources.LoadAll<FarmingPlantConfig>("Configs/FarmingPlants").ToList();
        ResourcesTable = Resources.LoadAll<ResourcesTable>("Configs").FirstOrDefault();
        ResourcesConfig = Resources.LoadAll<ResourcesConfig>("Configs").FirstOrDefault();
    }
}