using TMPro;
using UnityEngine;

public class BuildVersionText : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _buildText;

    private void Start() {
        _buildText.text = $"Ver. {Application.version}";
    }
}