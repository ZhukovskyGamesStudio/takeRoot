using System.Collections.Generic;

public interface IFarmingService : IService {
    public List<FarmingPlantConfig> AvailableFarmingPlantConfigs { get;  }
}
