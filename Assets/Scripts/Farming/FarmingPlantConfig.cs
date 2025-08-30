using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FarmingPlantConfig", menuName = "Scriptable Objects/FarmingPlantConfig", order = 0)]
public class FarmingPlantConfig : ScriptableObject {
    [field:SerializeField]
    public FarmingPlantType PlantType { get; set; }
    
    [field: SerializeField]
    public MainInfoData MainData { get; set; }

    [field: SerializeField]
    public List<Sprite> Sprites { get; set; }

    [field: SerializeField]
    public AYellowpaper.SerializedCollections.SerializedDictionary<float, Sprite> GrowthStages { get; set; }

    public Sprite GetGrowthSpriteByLevel(float growthLevel) {
        int i = 0;
        foreach (var kvp in GrowthStages) {
            if(kvp.Key > growthLevel) {
                return GrowthStages.ElementAt(i - 1).Value;
            }

            i++;
        }
        return GrowthStages.ElementAt(GrowthStages.Count - 1).Value;
    }
}