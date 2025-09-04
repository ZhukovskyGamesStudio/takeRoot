using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OccurenceConfig", menuName = "Scriptable Objects/OccurenceConfig", order = 0)]
public class OccurenceConfig : ScriptableObject {
    public OccurenceType Type;
    public int DifficultyCost;

    public List<GameObject> RandomPrefabToSpawn;


    public GameObject GetRandomizeToSpawn => RandomPrefabToSpawn[Random.Range(0, RandomPrefabToSpawn.Count)];
}