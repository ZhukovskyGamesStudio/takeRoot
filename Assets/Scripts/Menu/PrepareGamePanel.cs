using System;
using System.Collections;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PrepareGamePanel : MonoBehaviour {
    [SerializeField]
    private Animation _animation;

    [SerializeField]
    private AnimationClip _show, _hide, _startServer, _stopServer, _startJoin, _stopJoin;

    [SerializeField]
    private TextMeshProUGUI _codeText;

    [SerializeField]
    private NetworkObject _networkCanvas;

    [SerializeField]
    private ChooseRacePanel _chooseRacePanel;
    [SerializeField]
    private PlayerRaceSelection _playerRaceSelection;
    
    private State _state = State.Choosing;

    private string _serverCode;

    public void Show() {
        gameObject.SetActive(true);
        _animation.Play(_show.name);
    }

    public void Back() {
        switch (_state) {
            case State.Choosing:
                Close();
                break;
            case State.Hosting:
                StopHosting();
                break;
            case State.Joining:
                StopJoining();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Close() {
        _animation.Play(_hide.name);
        StartCoroutine(DisableAfterHide());
    }

    private void StopHosting() {
        _state = State.Choosing;
        _animation.Play(_stopServer.name);
        NetworkManager.Singleton.Shutdown();
    }

    private void StopJoining() {
        _state = State.Choosing;
        _animation.Play(_stopJoin.name);
        NetworkManager.Singleton.Shutdown();
    }

    public void Host() {
        _state = State.Hosting;
        _animation.Play(_startServer.name);
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(_networkCanvas);
        _serverCode = IpConnection.GetLocalIPAddress();
        _codeText.text = _serverCode;
        SpawnDataHolder();
    }

    [ServerRpc]
    private void SpawnDataHolder() {
        if (FindAnyObjectByType<PlayerRaceSelection>() == null) {
            NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(_playerRaceSelection.GetComponent<NetworkObject>());
        }
    }

    private IEnumerator DisableAfterHide() {
        yield return new WaitWhile(() => _animation.isPlaying);
        gameObject.SetActive(false);
    }

    private void Update() {
        if (_state != State.Hosting) {
            return;
        }

        if (NetworkManager.Singleton.ConnectedClients.Count > 1) {
            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessageToAll(nameof(OpenChooseRace),
                new FastBufferWriter(1024, Allocator.Temp));
            OpenChRc();
        }
    }

    private void OpenChooseRace(ulong senderClientId, FastBufferReader reader) {
        // Read the data sent by the server
        //reader.ReadValueSafe(out string receivedMessage);
        //Debug.Log($"Received message from server: {receivedMessage}");
        OpenChRc();
    }

    private void OpenChRc() {
        gameObject.SetActive(false);
        _chooseRacePanel.gameObject.SetActive(true);
    }

    public void Join() {
        _state = State.Joining;
        _animation.Play(_startJoin.name);
    }

    public void CopyHostCode() {
        GUIUtility.systemCopyBuffer = _serverCode;
        //TODO popup ваш код скопирован
        //TODO ensure works on MAC
    }

    public void OnCodeInputted(string code) {
        //TODO start joining with this code
        Client(code);
    }

    public void Client(string serverIp) {
        IpConnection.SetIpAddress(serverIp);
        NetworkManager.Singleton.StartClient();
        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(nameof(OpenChooseRace), OpenChooseRace);
    }

    private enum State {
        Choosing,
        Hosting,
        Joining
    }
}