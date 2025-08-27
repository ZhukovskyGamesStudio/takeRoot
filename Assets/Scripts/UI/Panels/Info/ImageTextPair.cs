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
}