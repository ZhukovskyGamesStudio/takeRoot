using System;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwapToggle : Toggle {
    [SerializeField]
    private Image _targetImage;

    [SerializeField]
    private Sprite _onSprite, _offSprite;

    protected override void Start() {
        base.Start();

        if (_targetImage == null) {
            Debug.LogError("Target Image is not assigned.");
            return;
        }

        // Initialize the sprite based on the current state
        UpdateSprite(isOn);

        // Add a listener to respond to state changes
        onValueChanged.AddListener(UpdateSprite);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        // Remove listener to avoid memory leaks
        onValueChanged?.RemoveListener(UpdateSprite);
    }

    private void UpdateSprite(bool isOnArg) {
        if (_targetImage != null) {
            _targetImage.sprite = isOnArg ? _onSprite : _offSprite;
        }
    }
}