using System.Collections.Generic;

public interface IFarmingService : IService {
    public List<FarmingPlantConfig> AvailableFarmingPlantConfigs { get;  }
    
    public List<FarmingPlot> FarmingPlots { get; set; }

    public void SelectPlantToPlant(FarmingPlantConfig config);

    public void SelectCut();

    public void DeselectAll();
}
