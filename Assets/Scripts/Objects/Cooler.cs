using System;
using UnityEngine;

[RequireComponent(typeof(WaterLevel))]
public class Cooler : MonoBehaviour {
    public float WaterLevelChange = 1;
    public float SatietyChange = 2;
    public bool HasWater => _waterLevel.CurrentWater.Value > 0;

    private WaterLevel _waterLevel;

    public Transform InteractPos;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Sprite _fullSprite, _emptySprite;

    private void Start() {
        _waterLevel = GetComponent<WaterLevel>();
    }

    private void Update() {
        UpdateWaterLevelView();
    }

    private void UpdateWaterLevelView() {
        _spriteRenderer.sprite = HasWater ? _fullSprite : _emptySprite;
    }

    public void DecreaseWater() {
        _waterLevel.ChangeWater(-WaterLevelChange);
    }
}