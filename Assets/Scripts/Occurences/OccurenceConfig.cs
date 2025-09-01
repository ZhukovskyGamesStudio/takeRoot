using UnityEngine;

[CreateAssetMenu(fileName = "OccurenceConfig", menuName = "Scriptable Objects/OccurenceConfig", order = 0)]
public class OccurenceConfig : ScriptableObject {
    public OccurenceType Type;
    public int DifficultyCost;

    public GameObject PrefabToSpawn;
}