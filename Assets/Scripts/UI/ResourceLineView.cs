using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceLineView : MonoBehaviour {
    [SerializeField]
    private Image _icon;

    [SerializeField]
    private TextMeshProUGUI _itemName, _itemAmount;

    [SerializeField]
    private ResourcesTable _resourcesTable;

    public void SetData(ResourceType type, int amount) {
        _icon.sprite = _resourcesTable.ResourceIconsDictionary[type];
        _itemName.text = type.ToString();
        _itemAmount.text = $"{amount} шт";
    }
}