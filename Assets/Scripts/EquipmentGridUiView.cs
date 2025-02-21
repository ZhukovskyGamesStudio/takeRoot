using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentGridUiView : MonoBehaviour {
    [SerializeField]
    private List<EquipmentUiView> _views;

    public void Set(SettlerData settlerData) {
        foreach (var VARIABLE in _views) {
            VARIABLE.ClearSlot();
        }

        foreach (var kvp in settlerData.Equipped) {
            var eType = kvp.Key;
            var v = _views.First(uv => uv.EquipmentType == eType);
            if (v != null) {
                v.Equip(kvp.Value);
            }
        }
    }

    public void Unequip(EquipmentType eType) {
        SettlersSelectionManager.Instance.SelectedSettler.Unequip(eType);
    }
}