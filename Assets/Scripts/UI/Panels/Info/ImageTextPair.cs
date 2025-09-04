using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageTextPair : MonoBehaviour {
    [SerializeField]
    private Image _image;

    [SerializeField]
    private TextMeshProUGUI _text;

    public void SetData(Sprite sprite, string text) {
        _image.sprite = sprite;
        _text.text = text;
    }

    public void SetDataUnavailable(Sprite sprite, string text) {
        _image.sprite = sprite;
        _text.text = text;
        _image.color = new Color(90f/255f, 90f/255f, 90f/255f, 212f);
    }
}