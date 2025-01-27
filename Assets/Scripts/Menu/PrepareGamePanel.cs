using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PrepareGamePanel : MonoBehaviour {
    [SerializeField]
    private NetworkManager _networkManager;
    [SerializeField]
    private Animation _animation;

    [SerializeField]
    private AnimationClip _show, _hide, _startServer, _stopServer, _startJoin, _stopJoin;

    [SerializeField]
    private TextMeshProUGUI _codeText;

    [SerializeField]
    private NetworkObject _networkCanvas;

    private State _state = State.Choosing;

    private string _serverCode;

    public void Show() {
        gameObject.SetActive(true);
        _animation.Play(_show.name);
    }

    public void Back() {
        switch (_state) {
            case State.Choosing:
                Close(); break;
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
    }

    private void StopJoining() {
        _state = State.Choosing;
        _animation.Play(_stopJoin.name);
    }
    
    public void Host() {
        _state = State.Hosting;
        _animation.Play(_startServer.name);
        _networkManager.StartHost();
        _networkManager.SpawnManager.InstantiateAndSpawn(_networkCanvas);
        _codeText.text = IpConnection.GetLocalIPAddress();
    }

    private IEnumerator DisableAfterHide() {
        yield return new WaitWhile(() => _animation.isPlaying);
        gameObject.SetActive(false);
    }

    private void Update() {
        if (_state != State.Hosting) {
            return;
        }

        if (_networkManager.ConnectedClients.Count > 1) {
            Menu.Instance.Play();
            _state = State.Choosing;
        }
       
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
        _networkManager.StartClient();
    }
    
    private enum State {
        Choosing,
        Hosting,
        Joining
    }
    
}
