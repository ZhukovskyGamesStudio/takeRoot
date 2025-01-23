using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourcesTable", menuName = "Scriptable Objects/ResourcesTable")]
public class ResourcesTable : ScriptableObject {
    [field: SerializeField]
    public List<ResourceView> ResourceViewPrefabs { get; private set; } = new();

    [field: SerializeField]
    public List<ResouseUiView> ResourceUiViewPrefabs { get; private set; } = new();
}