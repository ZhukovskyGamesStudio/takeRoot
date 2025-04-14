using UnityEngine;
using UnityEngine.UI;

public class EquipmentUiView : ResouseUiView {
    [SerializeField]
    private EquipmentGridUiView _equipmentGridUi;

    [field: SerializeField]
    public EquipmentType EquipmentType;

    [SerializeField]
    private Image _icon;

    public void Equip(ResourceType resourceType) {
        _icon.gameObject.SetActive(true);
        _icon.sprite = Core.ResourceManager.EquipmentIcons[resourceType];
    }

    public void ClearSlot() {
        _icon.gameObject.SetActive(false);
    }

    //TODO fix multihammers bug
    public void Unequip() {
        if (!_icon.gameObject.activeSelf) {
            return;
        }

        ClearSlot();
        _equipmentGridUi.Unequip(EquipmentType);
    }
}