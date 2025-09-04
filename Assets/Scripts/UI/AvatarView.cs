using System;
using UnityEngine;
using UnityEngine.UI;

public class AvatarView : MonoBehaviour {
    [SerializeField]
    private Image _bgImage, _gradientImage, _iconImage;

    [SerializeField]
    private Sprite _chamomile, _succulent, _toster, _lamp;

    [SerializeField]
    private Gradient _stressGradient, _hpGradient;

    [SerializeField]
    private Toggle _toggle;

    private AI.Settler _settler;
    private Action<SettlerSelectable> _onSelectSettler;

    public void Init(AI.Settler settlerData, Action<SettlerSelectable> onSelectSettler, ToggleGroup toggleGroup) {
        _settler = settlerData;
        _onSelectSettler = onSelectSettler;
        _toggle.group = toggleGroup;
        UpdateData();
    }

    public void UpdateData() {
        if (_settler.Data.Dead) {
            Destroy(gameObject);
            return;
        }

        _iconImage.sprite = _settler.Data.names.Subrace switch {
            Subrace.Chamomile => _chamomile,
            Subrace.Succulent => _succulent,
            Subrace.Toster => _toster,
            Subrace.Lamp => _lamp,
            _ => _iconImage.sprite
        };

        Settler_Needs needs = _settler.Data.needs.Value;
        float stressPercent = (float)needs.StressData.currentStress / needs.StressData.maxStress;
        float hpPercent = (float)needs.Hp / needs.MaxHp;

        _bgImage.color = _stressGradient.Evaluate(stressPercent);
        _gradientImage.color = _hpGradient.Evaluate(hpPercent);
    }

    public void Select(bool isOn) {
        if (!isOn) {
            return;
        }

        _onSelectSettler?.Invoke(_settler.GetComponent<SettlerSelectable>());
    }
}