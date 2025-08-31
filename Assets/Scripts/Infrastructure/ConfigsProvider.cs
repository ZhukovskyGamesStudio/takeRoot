using System.Collections.Generic;
using System.Linq;
using GameResources;
using UnityEngine;

public class ConfigsProvider : IConfigsProvider {
    public List<FarmingPlantConfig> FarmingConfigs { get; set; }
    public ResourcesTable ResourcesTable { get; set; }
    public ResourcesConfig ResourcesConfig { get; set; }
    public BuildingsConfig BuildingsConfig { get; set; }
    public ResearchConfig ResearchConfig { get; set; }
    public TimeScaleConfig TimeScaleConfig { get; set; }
    public CameraMovementConfig CameraMovementConfig { get; set; }
    public WorldConfig WorldConfig { get; set; }
    public IngameTimeConfig IngameTimeConfig { get; set; }

    public ConfigsProvider() {
        CacheConfigs();
    }

    private void CacheConfigs() {
        FarmingConfigs = Resources.LoadAll<FarmingPlantConfig>("Configs/FarmingPlants").ToList();
        ResourcesTable = Resources.LoadAll<ResourcesTable>("Configs").FirstOrDefault();
        ResourcesConfig = Resources.LoadAll<ResourcesConfig>("Configs").FirstOrDefault();
        BuildingsConfig = Resources.LoadAll<BuildingsConfig>("Configs").FirstOrDefault();
        ResearchConfig = Resources.LoadAll<ResearchConfig>("Configs").FirstOrDefault();
        TimeScaleConfig = Resources.LoadAll<TimeScaleConfig>("Configs").FirstOrDefault();
        CameraMovementConfig = Resources.LoadAll<CameraMovementConfig>("Configs").FirstOrDefault();
        WorldConfig = Resources.LoadAll<WorldConfig>("Configs").FirstOrDefault();
        IngameTimeConfig = Resources.LoadAll<IngameTimeConfig>("Configs").FirstOrDefault();
    }
}