public interface IWorldWriter : IService {
    void AddScaleGlobalEnergyChangeMultiplier(string key, float scale);
    void RemoveScaleGlobalEnergyChangeMultiplier(string key, float scale);
}