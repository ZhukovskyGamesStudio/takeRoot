using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameResources {
    [CreateAssetMenu(fileName = "ResourcesConfig", menuName = "GameResources/ResourcesConfig")]
    public class ResourcesConfig : ScriptableObject {
        public List<Resource> ResourcesPrefabs;
        public AYellowpaper.SerializedCollections.SerializedDictionary<ResourceType, int> ResourceWeights;
        public int TotalWeight => ResourceWeights.Select(r => r.Value).Sum();
        
    }
}