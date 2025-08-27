using TMPro;
using UnityEngine;

public class MainInfoPart : MonoBehaviour {
    [SerializeField]
    private ImageTextPair _mainIconText;

    [SerializeField]
    private TextMeshProUGUI _descriptionText, _hpText;

    private MainInfoData _infoData;

    public void SetData(MainInfoData infoData) {
        _infoData = infoData;
        _mainIconText.SetData(infoData.Icon, infoData.Name);
        _descriptionText.SetText(infoData.Description);
    }
}