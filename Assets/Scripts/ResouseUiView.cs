using TMPro;
using UnityEngine;

public class ResouseUiView : MonoBehaviour {
    [field: SerializeField]
    public ResourceType ResourceType { get; private set; }

    [Space(20)]
    [SerializeField]
    private TextMeshProUGUI _amountText;

    public int Amount { get; private set; }

    public void SetAmount(int amount) {
        _amountText.text = amount.ToString();
        Amount = amount;
    }
    public void SetAmount(int amount, string text)
    {
        _amountText.text = text;
        Amount = amount;
    }
}