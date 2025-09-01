using UnityEngine;

[RequireComponent(typeof(ElectricityLevel))]
public class DinamoMachine : MonoBehaviour {
    public IDinamoCharger DinamoCharger;
    public float ElectricityChange = 1;

    [Range(0, 1)]
    public float LowElectricityThreshold = 0.3f;

    [Range(0, 1)]
    public float HighElectricityThreshold = 1f;

    public bool HasElectricity => _electricityLevel.CurrentElectricity.Value > 0;

    public bool LowElectricity => _electricityLevel.CurrentElectricity.Value / _electricityLevel.maxElectricity <= LowElectricityThreshold;
    public bool HighElectricity => _electricityLevel.CurrentElectricity.Value / _electricityLevel.maxElectricity >= HighElectricityThreshold;

    public bool IsFree => DinamoCharger == null;

    private ElectricityLevel _electricityLevel;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Sprite _fullSprite, _emptySprite;

    private void Start() {
        _electricityLevel = GetComponent<ElectricityLevel>();
    }

    private void Update() {
        UpdateElectricityLevelView();
    }

    private void UpdateElectricityLevelView() {
        // _spriteRenderer.sprite = HasWater ? _fullSprite : _emptySprite;
    }

    public void DecreaseCharge() {
        _electricityLevel.ChangeElecticity(-ElectricityChange);
    }

    public void Charge() {
        _electricityLevel.ChangeElecticity(ElectricityChange);
    }

    public void PlayWorkAnimation() {
        GetComponent<Animator>().SetTrigger("Work");
    }

    public void PlayIdleAnimation() {
        GetComponent<Animator>().SetTrigger("Idle");
    }
}