using System.Collections.Generic;
using UnityEngine;

namespace GameResources {
    [CreateAssetMenu(fileName = "ResourcesConfig", menuName = "GameResources/ResourcesConfig")]
    public class ResourcesConfig : ScriptableObject {
        public List<Resource> ResourcesPrefabs;
    }
}