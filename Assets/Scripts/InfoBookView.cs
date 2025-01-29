using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoBookView : MonoBehaviour {
    [SerializeField]
    private Image _icon;

    [SerializeField]
    private TextMeshProUGUI _nameText;

    [SerializeField]
    private ResourceGridView _resourceGridView;

    public void Init(InfoBookData data) {
        _icon.sprite = data.Icon;
        _nameText.text = data.Name;

        _resourceGridView.FillGrid(data.Resources);
    }
}

public class InfoBookData {
    public Sprite Icon;
    public string Name;

    public List<ResourceData> Resources;
}