using System.Collections.Generic;
using System.Linq;

public class WorldState : IWorldReader, IWorldWriter {
    private float _baseEnergyChangeMultiplier;
    private readonly Dictionary<string, float> _energyChangeModifiers = new(5);
    private WorldConfig _worldConfig;
    public float GlobalEnergyChangeMultiplier =>
        _baseEnergyChangeMultiplier * _energyChangeModifiers.Values.Aggregate(1f, (acc, f) => acc * f);

    public WorldState(IConfigsProvider config) {
        _worldConfig = config.WorldConfig;
        //_baseEnergyChangeMultiplier = config.BaseEnergyChangeMultiplier;
    }

    public void AddScaleGlobalEnergyChangeMultiplier(string key, float scale) {
        _energyChangeModifiers[key] = scale;
    }

    public void RemoveScaleGlobalEnergyChangeMultiplier(string key, float scale) {
        _energyChangeModifiers.Remove(key);
    }
}