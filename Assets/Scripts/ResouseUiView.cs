using TMPro;
using UnityEngine;

public class ResouseUiView : MonoBehaviour {
    public int Amount { get; private set; }

    [field: SerializeField]
    public ResourceType ResourceType { get; private set; }

    [Space(20)]
    [SerializeField]
    private TextMeshProUGUI _amountText;

   

    public void SetAmount(int amount) {
        _amountText.text = amount.ToString();
        Amount = amount;
    }
}