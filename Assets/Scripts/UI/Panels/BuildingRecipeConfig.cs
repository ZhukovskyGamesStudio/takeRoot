using UnityEngine;

[CreateAssetMenu(fileName = "BuildingRecipeConfig", menuName = "Scriptable Objects/BuildingRecipeConfig", order = 0)]
public class BuildingRecipeConfig : ScriptableObject {
    [Header("Shouldn't be here!!!")]
    public string HeaderName;

    public Sprite Icon;
    public string Description;
    public Vector2Int Footprint;
    public int Hp;

    [Header("Normal Values")]
    public BuildingCategory BuildingCategory;

    public AYellowpaper.SerializedCollections.SerializedDictionary<ResourceType, int> Ingridients;
}