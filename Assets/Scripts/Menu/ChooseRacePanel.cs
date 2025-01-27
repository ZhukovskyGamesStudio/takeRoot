using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChooseRacePanel : MonoBehaviour {
    [SerializeField]
    private Button _readyButton;

    [SerializeField]
    private TextMeshProUGUI _readyAmount;

    private PlayerRaceSelection _playerRaceSelection;

    private void Start() {
        _playerRaceSelection = FindAnyObjectByType<PlayerRaceSelection>();
    }

    public void ChooseRace(int raceIndex) {
        Race race = (Race)raceIndex;
        _playerRaceSelection.ChooseClientRaceServerRpc(NetworkManager.Singleton.IsHost ? 1 : 2, race);
    }

    public void SetReady() {
        _playerRaceSelection.SetClientReadyServerRpc(NetworkManager.Singleton.IsHost ? 1 : 2);
    }

    private void Update() {
        if (_playerRaceSelection.Player1Ready.Value && _playerRaceSelection.Player2Ready.Value) {
            gameObject.SetActive(false);
            Menu.Instance.Play();
        }

        if (_playerRaceSelection.Player2Race.Value != Race.None && _playerRaceSelection.Player1Race.Value != Race.None) {
            _readyButton.gameObject.SetActive(true);
            _readyButton.interactable = _playerRaceSelection.Player2Race.Value != _playerRaceSelection.Player1Race.Value;
        } else {
            _readyButton.gameObject.SetActive(false);
        }

        UpdateReadyAmount();
    }

    private void UpdateReadyAmount() {
        int amount = 0;
        if (_playerRaceSelection.Player1Ready.Value) {
            amount++;
        }

        if (_playerRaceSelection.Player2Ready.Value) {
            amount++;
        }

        _readyAmount.text = $"{amount}/2";
    }
}

[Serializable]
public enum Race {
    None = 0,
    Plants = 1,
    Robots = 2,
    Both = 3
}