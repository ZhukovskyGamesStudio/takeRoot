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
    private MainMenuPanel _mainMenuPanel;

    [SerializeField]
    private AnimationClip _show, _hide, _startServer, _stopServer, _startJoin, _stopJoin;

    [SerializeField]
    private TextMeshProUGUI _codeText;

    [SerializeField]
    private NetworkObject _networkCanvas;

    [SerializeField]
    private ChooseRacePanel _chooseRacePanel;

    [SerializeField]
    private NetworkDataHolder _networkDataHolder;

    private State _state = State.Choosing;

    private string _serverCode;

    public void Show() {
        BackgroundParallax.IsParallaxDisabled = true;
        gameObject.SetActive(true);
        _animation.Play(_show.name);
    }

    public void Back() {
        switch (_state) {
            case State.Choosing:
                Close();
                _mainMenuPanel.Open();
                BackgroundParallax.IsParallaxDisabled = false;
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
        SteamSessionService.Instance?.LeaveLobby();
    }

    private void StopJoining() {
        _state = State.Choosing;
        _animation.Play(_stopJoin.name);
        NetworkManager.Singleton.Shutdown();
        SteamSessionService.Instance?.LeaveLobby();
    }

    public void Host() {
        BeginHost(TransportKind.Unity);
        _serverCode = NetworkBootstrap.GetLocalIp();
        _codeText.text = _serverCode;
    }

    // Steam host: start the listen-server on the Steam transport and open a Friends-only lobby so a
    // friend can be invited via the overlay. No IP code — the invite itself carries the connection.
    public void HostViaSteam() {
        BeginHost(TransportKind.Steam);
        SteamSessionService.Instance.HostLobby();
    }

    public void InviteFriend() {
        SteamSessionService.Instance.OpenInviteOverlay();
    }

    private void BeginHost(TransportKind kind) {
        _state = State.Hosting;
        _animation.Play(_startServer.name);
        NetworkBootstrap.StartHost(kind);
        NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(_networkCanvas);
        SpawnDataHolder();
    }

    [ServerRpc]
    private void SpawnDataHolder() {
        if (FindAnyObjectByType<NetworkDataHolder>() == null) {
            NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(_networkDataHolder.GetComponent<NetworkObject>());
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
        NetworkBootstrap.StartClient(ConnectTarget.ForIp(serverIp));
        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(nameof(OpenChooseRace), OpenChooseRace);
    }

    // Entered from SteamSessionService when an accepted invite / lobby join resolves to a host
    // SteamID. Mirrors Client() but connects over the Steam transport instead of an IP.
    public void JoinViaSteam(ulong hostSteamId) {
        _state = State.Joining;
        NetworkBootstrap.StartClient(ConnectTarget.ForSteam(hostSteamId));
        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(nameof(OpenChooseRace), OpenChooseRace);
    }

    private enum State {
        Choosing,
        Hosting,
        Joining
    }
}