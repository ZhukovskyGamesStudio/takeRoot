using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class InputtedStringControl : NetworkBehaviour {
    private string _curInputtedString = "";

    [SerializeField]
    private PlayerHeadCanvas _playerHeadCanvas;
    public string InputtedString {
        get { return _curInputtedString; }
        set {
            if (_curInputtedString != value) {
                _playerHeadCanvas.Text.text = value;
            }

            _curInputtedString = value;
        }
    }

    private void Awake() {
        AddText("Privet");
    }

    private void Update() {
        if (IsOwner) {
            if (Input.inputString.Length > 0) {
                AddText(Input.inputString);
            }
        }
    }

    private void LateUpdate() {
        _playerHeadCanvas.transform.forward = Camera.main.transform.forward;
    }

    private void AddText(string txt) {
        InputtedString += txt;
        StartCoroutine(ClearStrCoroutine(txt.Length));
    }

    private IEnumerator ClearStrCoroutine(int lettersAmount) {
        yield return new WaitForSeconds(3);
        InputtedString = _curInputtedString.Substring(lettersAmount);
    }
}