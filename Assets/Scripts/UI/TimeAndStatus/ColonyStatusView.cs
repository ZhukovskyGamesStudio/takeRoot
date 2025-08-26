using TMPro;
using UnityEngine;

public class ColonyStatusView : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _settlersCountText, _maxStressText;


    public void SetData(int settlersCount, int maxStress) {
        _settlersCountText.text = settlersCount.ToString();
        _maxStressText.text = maxStress.ToString();
    }
}
