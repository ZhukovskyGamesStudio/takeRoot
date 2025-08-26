using System;
using TMPro;
using UnityEngine;
using AI;
using UnityEngine.UI;

public class SettlerInfoPanel : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _nameText, _typeText, _conditionText;

    [SerializeField]
    private GameObject _nameContainer, _editNameContainer;

    [SerializeField]
    private TMP_InputField _nameInput;

    [SerializeField]
    private Image _hpFill, _stressFill, _conditionIcon;

    [SerializeField]
    private Slider _hpSlider, _stressSlider, _hungerSlider, _energySlider, _conditionSlider;

    [SerializeField]
    private Gradient _hpGradient, _stressGradient;

    [SerializeField]
    private SerializedDictionary<SettlerCondition, Sprite> _conditionSprites;

    private AI.SettlerData _settlerData;
    private Action _onClose;

    public void SetData(AI.SettlerData settlerData, Action onClose) {
        _settlerData = settlerData;

        _onClose = onClose;

        _nameContainer.SetActive(true);
        _editNameContainer.SetActive(false);

        UpdateData();
    }

    public void Close() {
        _onClose.Invoke();
    }

    public void UpdateData() {
        _nameText.text = _settlerData.names.Name;

        float hp = (float)_settlerData.needs.Hp / _settlerData.needs.MaxHp;
        float stress = 1 - (float)_settlerData.needs.Stress / _settlerData.needs.MaxStress;
        float hunger = 1 - (float)_settlerData.needs.Hunger / _settlerData.needs.MaxHunger;
        float energy = (float)_settlerData.energy.currentEnergy / _settlerData.energy.maxEnergy;
        float condition = (float)_settlerData.needs.Care / _settlerData.needs.MaxCare;

        _hpSlider.value = hp;
        _stressSlider.value = stress;
        _hungerSlider.value = hunger;
        _energySlider.value = energy;
        _conditionSlider.value = condition;

        _hpFill.color = _hpGradient.Evaluate(hp);
        _stressFill.color = _stressGradient.Evaluate(stress);

        _typeText.text = _settlerData.names.Subrace switch {
            Subrace.Chamomile => "Ромашка",
            Subrace.Succulent => "Суккулент",
            Subrace.Toster => "Тостер",
            Subrace.Lamp => "Лампа",

            _ => string.Empty
        };
        _conditionText.text = _settlerData.Condition switch {
            SettlerCondition.Neutral => "Нейтральное",
            SettlerCondition.Sleep => "Сон",
            SettlerCondition.Breakdown => "Нервный срыв",
            SettlerCondition.Inspiration => "Вдохновение",

            _ => string.Empty
        };

        _conditionIcon.sprite = _conditionSprites[_settlerData.Condition];
    }

    public void StartEdit() {
        _nameContainer.SetActive(false);
        _editNameContainer.SetActive(true);

        _nameInput.text = _nameText.text;
    }

    public void EndEdit(bool apply) {
        _nameContainer.SetActive(true);
        _editNameContainer.SetActive(false);

        if (_nameInput.text == string.Empty || !apply) {
            return;
        }

        _nameText.text = _nameInput.text;
        _settlerData.names.Name = _nameInput.text;
    }
}