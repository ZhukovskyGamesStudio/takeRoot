using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using UnityEngine;

public class TimeMachine : MonoBehaviour {
    [SerializeField]
    public List<Transform> InteractPos;

    [SerializeField]
    private Progress _progressBar;

    [SerializeField]
    private int _neededProgress;

    public Dictionary<Race, AI.Settler> Chargers;

    private int _progress;
    private Dictionary<Race, bool> _chargedRace;
    private ITimeScaleService _timeScaleService;

    public bool Charged => _progress >= _neededProgress;

    private void Start() {
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

        _progress++;
        _progressBar.ProgressData.Progress.Value = _progress;

        if (Charged) {
            GetComponent<Animator>().SetTrigger("Work");
            _timeScaleService.SetReadyToPause();
        }
    }

    public void Use() {
        GetComponent<Animator>().SetTrigger("Idle");
        _progress = 0;
        _progressBar.ProgressData.Progress.Value = 0;
    }
}