using System.Collections.Generic;
using GameResources;

public interface IConfigsProvider : IService {
    public List<FarmingPlantConfig> FarmingConfigs { get; set; }

    public ResourcesTable ResourcesTable { get; set; }

    public ResourcesConfig ResourcesConfig { get; set; }
    public BuildingsConfig BuildingsConfig { get; set; }
    public ResearchConfig ResearchConfig { get; set; }
    public TimeScaleConfig TimeScaleConfig { get; set; }
    public CameraMovementConfig CameraMovementConfig { get; set; }
    public WorldConfig WorldConfig { get; set; }
    public IngameTimeConfig IngameTimeConfig { get; set; }
}