using System;
using TMPro;
using UnityEngine;

public class PrepareGamePanel : MonoBehaviour {
    [SerializeField]
    private Animation _animation;

    [SerializeField]
    private AnimationClip _show, _hide, _startServer, _stopServer, _startJoin, _stopJoin;

    [SerializeField]
    private TextMeshProUGUI _codeText;

    private State _state = State.Choosing;

    private string _serverCode;

    public void Show() {
        _animation.Play(_show.ToString());
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
        _animation.Play(_hide.ToString());
    }

    private void StopHosting() {
        _state = State.Choosing;
        _animation.Play(_stopServer.ToString());
    }

    private void StopJoining() {
        _state = State.Choosing;
        _animation.Play(_stopJoin.ToString());
    }
    
    public void Host() {
        _state = State.Hosting;
        _animation.Play(_startServer.ToString());
    }

    public void Join() {
        _state = State.Joining;
        _animation.Play(_startJoin.ToString());
    }

    public void CopyHostCode() {
        GUIUtility.systemCopyBuffer = _serverCode;
        //TODO popup ваш код скопирован
        //TODO ensure works on MAC
    }

    public void OnCodeInputted(string code) {
        //TODO start joining with this code
    }
    
    private enum State {
        Choosing,
        Hosting,
        Joining
    }
    
}
