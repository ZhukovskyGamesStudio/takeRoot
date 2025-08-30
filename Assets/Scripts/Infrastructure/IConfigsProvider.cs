using System.Collections.Generic;

public interface IConfigsProvider : IService {
    public List<FarmingPlantConfig> FarmingConfigs { get; set; }
}
