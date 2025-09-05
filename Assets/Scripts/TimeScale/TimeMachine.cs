using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using Unity.Netcode;
using UnityEngine;

public class TimeMachine : NetworkBehaviour {
    [SerializeField]
    public List<Transform> InteractPos;

    [SerializeField]
    private Progress _progressBar;

    [SerializeField]
    private int _neededProgress = 100;

    [SerializeField]
    private float _chargingSpeed = 5;

    public Dictionary<Race, AI.Settler> Chargers;

    private float _progress;
    private Dictionary<Race, bool> _chargedRace;
    private ITimeScaleService _timeScaleService;
    private CommandTarget _commandTarget;

    public bool Charged => _progress >= _neededProgress;

    private void Start() {
        _commandTarget = GetComponent<CommandTarget>();
        Chargers = new Dictionary<Race, AI.Settler> {
            { Race.Plants, null },
            { Race.Robots, null }
        };

        _chargedRace = new Dictionary<Race, bool> {
            { Race.Plants, false },
            { Race.Robots, false }
        };

        _progressBar.ProgressData.Needed = _neededProgress;

        _timeScaleService = ServiceLocator.Container.Single<ITimeScaleService>();
        _timeScaleService.SetTimeMachine(this);
    }

    public void Charge(Race race) {
        _chargedRace[race] = true;

        foreach (KeyValuePair<Race, bool> kvp in _chargedRace) {
            if (!kvp.Value) return;
        }

        foreach (Race key in _chargedRace.Keys.ToArray()) {
            _chargedRace[key] = false;
        }

        SetProgress(_progress + _chargingSpeed);
        SetProgressClientRpc(_progress);

        if (Charged) {
            _commandTarget.SetPerform(true);
            _timeScaleService.SetReadyToPause();
            SetPauseEnabledClientRpc();
        }
    }

    [ClientRpc]
    private void SetPauseEnabledClientRpc() {
        _timeScaleService.SetReadyToPause();
    }

    public void Use() {
        _commandTarget.SetPerform(false);
        
        SetProgress(0);
        SetProgressClientRpc(_progress);
    }

    [ClientRpc]
    private void SetProgressClientRpc(float value) {
        SetProgress(value);
    }

    private void SetProgress(float value) {
        _progress = value;
        _progressBar.ProgressData.Progress.Value = Mathf.RoundToInt(_progress);
    }
}