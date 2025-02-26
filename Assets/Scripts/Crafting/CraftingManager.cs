using System.Collections.Generic;
using System.Linq;
using Settlers.Crafting;
using UnityEngine;

public class CraftingManager : MonoBehaviour, IInitableInstance {
    [SerializeField]
    private List<CraftingReceiptConfig> _receipts;

    public void Init() {
        Core.CraftingManager = this;
    }

    public CraftingReceiptConfig GetReceipt(string uid) {
        return _receipts.FirstOrDefault(r => r.ReceiptUid == uid);
    }
}