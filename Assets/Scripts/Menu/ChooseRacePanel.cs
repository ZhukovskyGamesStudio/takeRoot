using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChooseRacePanel : MonoBehaviour {
    [SerializeField]
    private Button _readyButton, _plantsButton, _robotsButton;

    [SerializeField]
    private Image _plantsImage, _robotsImage;

    [SerializeField]
    private TextMeshProUGUI _readyAmount;

    private NetworkDataHolder NetworkDataHolder => NetworkDataHolder.Instance;

    private string _readyAmountText;

    public void ChooseRace(int raceIndex) {
        Race race = (Race)raceIndex;
        NetworkDataHolder.ChooseClientRaceServerRpc(NetworkManager.Singleton.IsHost ? 1 : 2, race);

        _plantsImage.enabled = _robotsImage.enabled = false;
        _plantsButton.interactable = _robotsButton.interactable = true;

        if (race == Race.Plants) {
            _plantsImage.enabled = true;
            _plantsButton.interactable = false;
        }
        else if (race == Race.Robots) {
            _robotsImage.enabled = true;
            _robotsButton.interactable = false;
        }
    }

    public void SetReady() {
        NetworkDataHolder.SetClientReadyServerRpc(NetworkManager.Singleton.IsHost ? 1 : 2);
    }

    private void Awake() {
        _readyAmountText = _readyAmount.text;
    }

    private void Update() {
        if (NetworkDataHolder.SelectRaceData.HostReady.Value && NetworkDataHolder.SelectRaceData.ClientReady.Value) {
            gameObject.SetActive(false);
            MenuEntryPoint.Instance.Play();
        }

        if (NetworkDataHolder.MainGameNetworkData.ClientRace.Value != Race.None && NetworkDataHolder.MainGameNetworkData.HostRace.Value != Race.None) {
            _readyButton.gameObject.SetActive(true);
            _readyButton.interactable = NetworkDataHolder.MainGameNetworkData.ClientRace.Value != NetworkDataHolder.MainGameNetworkData.HostRace.Value;
        } else {
            _readyButton.gameObject.SetActive(false);
        }

        UpdateReadyAmount();
    }

    private void UpdateReadyAmount() {
        int amount = 0;
        if (NetworkDataHolder.SelectRaceData.HostReady.Value) {
            amount++;
        }

        if (NetworkDataHolder.SelectRaceData.ClientReady.Value) {
            amount++;
        }

        _readyAmount.text = _readyAmountText + $"{amount}/2";
    }
}

[Serializable]
public enum Race {
    None = 0,
    Plants = 1,
    Robots = 2,
    Both = 3
}