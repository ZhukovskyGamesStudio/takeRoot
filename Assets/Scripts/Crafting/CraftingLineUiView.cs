using Settlers.Crafting;
using TMPro;
using UnityEngine;

public class CraftingLineUiView : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _headerText, _explainText;

    [SerializeField]
    private TextMeshProUGUI _amountText;

    [SerializeField]
    private ResourceGridView _requiredResourcesGridView;

    public void Set(CraftingReceiptConfig config) {
        _headerText.text = config.ResultingResource.ResourceType.ToString();
        _explainText.text = config.ExplainText;
    }

    public void UpdateAmount(int queueAmount, int stockAmount) {
        _amountText.text = $"{queueAmount}/{stockAmount}";
    }

    public void AddOne() { }

    public void RemoveOne() { }
}