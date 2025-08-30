using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingRecipeConfig", menuName = "Scriptable Objects/BuildingRecipeConfig", order = 0)]
public class BuildingRecipeConfig : ScriptableObject {
    
    public MainInfoData mainInfo;
    
    public Sprite ObjectSprite;
   
    public Vector2Int Footprint;
    public int Hp;

    [Header("Normal Values")]
    public BuildingCategory BuildingCategory;

    public AYellowpaper.SerializedCollections.SerializedDictionary<ResourceType, int> Ingridients;
}