using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchReward : MonoBehaviour {
    [SerializeField]
    private Image _iconImage;

    [SerializeField]
    private TextMeshProUGUI _rewardText;

    public void Init(SpriteAndName data) {
        if (data == null) {
            _iconImage.enabled = false;
            _rewardText.text = "Нет";
            return;
        }

        _iconImage.sprite = data.Sprite;
        _rewardText.text = data.Name;
    }
}
