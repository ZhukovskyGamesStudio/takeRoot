using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FarmingPlantConfig", menuName = "Scriptable Objects/FarmingPlantConfig", order = 0)]
public class FarmingPlantConfig : ScriptableObject {
    [field: SerializeField]
    public MainInfoData MainData { get; set; }

    [field: SerializeField]
    public List<Sprite> Sprites{ get; set; }
}