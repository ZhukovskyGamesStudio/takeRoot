using System;
using CodeBase.Services;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class TimeStatusUI : NetworkBehaviour {
    [SerializeField]
    private GameSpeedView _gameSpeedView;

    [SerializeField]
    private DayStatusView _dayStatusView;

    [SerializeField]
    private ColonyStatusView _colonyStatusView;

    private Action<GameSpeedType, Race> _onChangeSpeed;

    public void SetData(IngameTimeData data, Action<GameSpeedType,Race> onChangeSpeed, ReactiveProperty<bool> isReadyToPause) {
        _onChangeSpeed = onChangeSpeed;
        _gameSpeedView.Init(ChangeSpeed,isReadyToPause);

        _dayStatusView.SetData(data);
    }

    public void Start() {
        _gameSpeedView.SetFriendSelectionClientRpc(Random.Range(0, 2) == 1 ? GameSpeedType.High : GameSpeedType.Normal);
        _colonyStatusView.SetData(Random.Range(0, 12), Random.Range(0, 101));
    }

    private void ChangeSpeed(GameSpeedType speed) {
        _onChangeSpeed.Invoke(speed, ServiceLocator.Container.Single<INetworkService>().MyRace.Value);
    }

    public void JumpToFriend() {
        Debug.LogWarning("Jump to Friend not implemented");
    }
}