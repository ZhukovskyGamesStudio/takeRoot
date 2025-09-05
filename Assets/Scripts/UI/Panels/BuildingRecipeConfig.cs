using UnityEngine;

[CreateAssetMenu(fileName = "BuildingRecipeConfig", menuName = "Scriptable Objects/BuildingRecipeConfig", order = 0)]
public class BuildingRecipeConfig : ScriptableObject {
    public MainInfoData mainInfo;

    public CommandTarget BuildingPrefab;

    public Vector2Int Footprint;
    public int Hp;

    public int RequiredBuildPoints;

    [Header("Normal Values")]
    public BuildingCategory BuildingCategory;

    public AYellowpaper.SerializedCollections.SerializedDictionary<ResourceType, int> Ingridients;
    
    public Research RequiredResearch;
}