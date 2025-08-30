using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TimeStatusUI : MonoBehaviour {
    [SerializeField]
    private GameSpeedView _gameSpeedView;

    [SerializeField]
    private DayStatusView _dayStatusView;

    [SerializeField]
    private ColonyStatusView _colonyStatusView;

    private Action<GameSpeedType> _onChangeSpeed;
    
    public void SetData(Action<GameSpeedType> onChangeSpeed) {
        _onChangeSpeed = onChangeSpeed;
        _gameSpeedView.Init(ChangeSpeed);
    }
    
    public void Start() {
        _gameSpeedView.SetFriendSelection(Random.Range(0, 2) == 1 ? GameSpeedType.High : GameSpeedType.Normal);
        _dayStatusView.SetData(Random.Range(0, 99), Random.Range(0, 2) == 1);
        _colonyStatusView.SetData(Random.Range(0, 12), Random.Range(0, 101));
    }
    
    private void ChangeSpeed(GameSpeedType speed) {
        _onChangeSpeed.Invoke(speed);
    }

    public void JumpToFriend() {
        Debug.LogWarning("Jump to Friend not implemented");
    }
}