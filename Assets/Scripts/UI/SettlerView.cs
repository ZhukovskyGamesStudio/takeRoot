using UnityEngine;
using UnityEngine.UI;

public class SettlerView : MonoBehaviour {
    [SerializeField]
    private Image _bgImage, _gradientImage, _iconImage;

    [SerializeField]
    private Sprite _chamomile, _succulent, _toster, _lamp;

    [SerializeField]
    private Color _minStressColor, _maxStressColor, _minHpColor, _maxHpColor;

    private AI.SettlerData _settlerData;

    public void Init(AI.SettlerData settlerData) {
        _settlerData = settlerData;

        UpdateData();
    }

    public void UpdateData() {
        _iconImage.sprite = _settlerData.names.Subrace switch {
            Subrace.Chamomile => _chamomile,
            Subrace.Succulent => _succulent,
            Subrace.Toster => _toster,
            Subrace.Lamp => _lamp,
            _ => _iconImage.sprite
        };

        var needs = _settlerData.needs;
        float stress = (float)needs.Stress / needs.MaxStress;
        float hp = (float)needs.Hp / needs.MaxHp;

        _bgImage.color = Color.Lerp(_minStressColor, _maxStressColor, stress);

        _gradientImage.enabled = hp < 0.9f;
        _gradientImage.color = Color.Lerp(_minHpColor, _maxHpColor, hp);
    }
}