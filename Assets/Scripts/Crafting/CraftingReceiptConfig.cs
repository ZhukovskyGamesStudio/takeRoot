using System.Collections.Generic;
using UnityEngine;

namespace Settlers.Crafting {
    [CreateAssetMenu(fileName = "CraftingReceiptConfig", menuName = "Scriptable Objects/CraftingReceiptConfig", order = 0)]
    public class CraftingReceiptConfig : ScriptableObject {
        [field: SerializeField]
        public string ReceiptUid;

        [field: SerializeField]
        public string ExplainText;

        [field: SerializeField]
        public List<ResourceData> RequiredResources { get; private set; }

        [field: SerializeField]
        public ResourceData ResultingResource { get; private set; }

        [field: SerializeField]
        public int CombinedCraftingPoints { get; private set; }
    }
}