using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentGridUiView : MonoBehaviour {
    [SerializeField]
    private List<EquipmentUiView> _views;

    public void Set(SettlerData settlerData) {
        foreach (EquipmentUiView VARIABLE in _views) {
            VARIABLE.ClearSlot();
        }

        foreach (KeyValuePair<EquipmentType, ResourceType> kvp in settlerData.Equipped) {
            EquipmentType eType = kvp.Key;
            EquipmentUiView v = _views.First(uv => uv.EquipmentType == eType);
            if (v != null) {
                v.Equip(kvp.Value);
            }
        }
    }

    public void Unequip(EquipmentType eType) {
        ObsoleteCoreEntryPoint.SettlersSelectionManager.SelectedSettler.Unequip(eType);
    }
}