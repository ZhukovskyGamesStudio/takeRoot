using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeadCanvas : MonoBehaviour {
    [SerializeField]
    private Slider _hpSlider, _energySlider, _boostSlider;

    [SerializeField]
    private TextMeshProUGUI _text;

    public TextMeshProUGUI Text => _text;

    public void SetData(PlayerNetwork.PlayerNetworkData data) {
        _hpSlider.value = data.HpPercent;
        _energySlider.value = data.EnergyPercent;
        _boostSlider.value = data.BoostPercent;
        _text.text = data.InputtedString.ToString();
    }
}