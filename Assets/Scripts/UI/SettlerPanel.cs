using TMPro;
using UnityEngine;
using AI;

public class SettlerPanel : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _nameText;

    [SerializeField]
    private GameObject _nameContainer, _editNameContainer;

    [SerializeField]
    private TMP_InputField _nameInput;

    private AI.SettlerData _settlerData;

    public void SetData(AI.SettlerData settlerData) {
        _settlerData = settlerData;

        _nameText.text = settlerData.names.Name;
    }

    public void StartEdit() {
        _nameContainer.SetActive(false);
        _editNameContainer.SetActive(true);

        _nameInput.text = _nameText.text;
    }

    public void EndEdit(bool apply) {
        _nameContainer.SetActive(true);
        _editNameContainer.SetActive(false);

        if (_nameInput.text == string.Empty || !apply) return;
        _nameText.text = _nameInput.text;
        _settlerData.names.Name = _nameInput.text;
    }
}