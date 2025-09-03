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

    private NetworkDataHolder NetworkDataHolder => NetworkDataHolder.Instance;

    public void ChooseRace(int raceIndex) {
        Race race = (Race)raceIndex;
        NetworkDataHolder.ChooseClientRaceServerRpc(NetworkManager.Singleton.IsHost ? 1 : 2, race);
    }

    public void SetReady() {
        NetworkDataHolder.SetClientReadyServerRpc(NetworkManager.Singleton.IsHost ? 1 : 2);
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