using UnityEngine;
using UnityEngine.UI;

public class SettlerView : MonoBehaviour {
    [SerializeField]
    private Image _bgImage, _gradientImage, _iconImage;

    [SerializeField]
    private Color _minStressColor, _maxStressColor, _minHpColor, _maxHpColor;
    
    private SettlerData _settlerData;
    
    public void Init(SettlerData settlerData) {
        _settlerData = settlerData;
        
        UpdateData();
    }

    public void UpdateData() {
        _iconImage.sprite = _settlerData.InfoBookIcon;

        float stress = (float)_settlerData.Stress / _settlerData.MaxStress;
        float hp = (float)_settlerData.Hp / _settlerData.MaxHp;
        
        _bgImage.color = Color.Lerp(_minStressColor, _maxStressColor, stress);

        _gradientImage.enabled = hp < 0.9f;
        _gradientImage.color = Color.Lerp(_minHpColor, _maxHpColor, hp);
    }
}
