using System;
using System.Linq;
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

    private void Update() {
        var settlers = ServiceLocator.Container.Single<ISettlersService>().MySettlers(NetworkDataHolder.GetRace());
        int maxStress = Mathf.RoundToInt(settlers.Max(s => s.Data.needs.Value.StressData.currentStress));
        _colonyStatusView.SetData(settlers.Count, 100-maxStress);
    }

    private void ChangeSpeed(GameSpeedType speed, Race race) {
        _onChangeSpeed.Invoke(speed, race);
    }

    public void JumpToFriend() {
        Debug.LogWarning("Jump to Friend not implemented");
    }
}