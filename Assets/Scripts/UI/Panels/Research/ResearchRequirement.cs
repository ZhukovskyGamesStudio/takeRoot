using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchRequirement : MonoBehaviour
{
    [SerializeField]
    private Image _iconImage;

    [SerializeField]
    private TextMeshProUGUI _requirementText;

    [SerializeField]
    private Color _requirementUndoneColor;

    [SerializeField]
    private Sprite _doneSprite, _undoneSprite;

    public void Init(string requirement, bool done) {
        _iconImage.sprite = done ? _doneSprite : _undoneSprite;
        _requirementText.text = requirement;
        if (!done) _requirementText.color = _requirementUndoneColor;
    }

    public void Init() {
        _iconImage.enabled = false;
        _requirementText.text = "Нет";
    }
}
