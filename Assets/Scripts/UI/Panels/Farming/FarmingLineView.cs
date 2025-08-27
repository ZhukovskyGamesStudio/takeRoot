using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmingLineView : MonoBehaviour {
    [SerializeField]
    private ImageTextPair _result;

    [SerializeField]
    private TextMeshProUGUI _explainText;

    [SerializeField]
    private List<Image> _gainImages, _gainImagesBack;

    private FarmingPlantConfig _config;
    private Action<FarmingPlantConfig> _onPlant;

    public void Set(FarmingPlantConfig config, Action<FarmingPlantConfig> onPlant) {
        _config = config;
        _onPlant = onPlant;
        _explainText.text = config.MainData.Description;
        _result.SetData(config.MainData.Icon, config.MainData.Name);

        UpdateSpritesList(config);
    }

    private void UpdateSpritesList(FarmingPlantConfig config) {
        foreach (var image in _gainImagesBack) {
            image.gameObject.SetActive(false);
        }

        for (int index = 0; index < config.Sprites.Count; index++) {
            Sprite icon = config.Sprites[index];
            _gainImages[index].sprite = icon;
            _gainImagesBack[index].gameObject.SetActive(true);
        }
    }

    public void Plant() {
        _onPlant?.Invoke(_config);
    }
}