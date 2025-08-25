using UnityEngine;
using UnityEngine.UI;

public class AvatarView : MonoBehaviour {
    [SerializeField]
    private Image _bgImage, _gradientImage, _iconImage;

    [SerializeField]
    private Sprite _chamomile, _succulent, _toster, _lamp;

    [SerializeField]
    private Gradient _stressGradient, _hpGradient;

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
        float stressPercent = (float)needs.Stress / needs.MaxStress;
        float hpPercent = (float)needs.Hp / needs.MaxHp;

        _bgImage.color = _stressGradient.Evaluate(stressPercent);
        _gradientImage.color = _hpGradient.Evaluate(hpPercent);
    }
}