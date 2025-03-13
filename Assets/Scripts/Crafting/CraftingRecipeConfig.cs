using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Settlers.Crafting {
    [CreateAssetMenu(fileName = "CraftingRecipeConfig", menuName = "Scriptable Objects/CraftingRecipeConfig", order = 0)]
    public class CraftingRecipeConfig : ScriptableObject {
        [field: SerializeField]
        public string RecipeUid;
        
        [field: SerializeField]
        public Sprite RecipeIcon;
        
        [field: SerializeField]
        public string ExplainText;

        [field: SerializeField] 
        public float CraftingPoints;
        
        [field: SerializeField]
        public List<ResourceData> RequiredResources { get; private set; }

        [field: SerializeField]
        public ResourceData ResultingResource { get; private set; }

        [field: SerializeField]
        public int CombinedCraftingPoints { get; private set; }
    }
}