using System.Collections.Generic;
using GameResources;

public interface IConfigsProvider : IService {
    public List<FarmingPlantConfig> FarmingConfigs { get; set; }

    public ResourcesTable ResourcesTable { get; set; }
    
    public ResourcesConfig ResourcesConfig { get; set; }
}
