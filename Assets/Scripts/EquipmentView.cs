using System;
using UnityEngine;

public class EquipmentView : MonoBehaviour {
    [field: SerializeField]
    public EquipmentType Type { get; private set; }

    public SpriteRenderer _handRenderer;
    public Sprite _emptyHand, _filledHand;
    public SpriteRenderer _equipmentRenderer;

    [SerializeField]
    private SerializedDictionary<ResourceType, Sprite> _itemsDict = new SerializedDictionary<ResourceType, Sprite>();

    public void SetEquipment(ResourceType type) {
        if (type == ResourceType.None) {
            _handRenderer.sprite = _emptyHand;
            _equipmentRenderer.enabled = false;
            return;
        }

        _handRenderer.sprite = _filledHand;
        _equipmentRenderer.sprite = _itemsDict[type];
    }
}