using System;
using System.Collections.Generic;
using CodeBase.Services;
using Unity.Netcode;
using UnityEngine;

public class ResearchStation : NetworkBehaviour {
    [SerializeField]
    private Progress _progressData;

    [SerializeField]
    private List<Research> _availableResearch;

    public bool HasResearch => _researches.GetResearchData().CurrentResearch != Research.None &&
                               _availableResearch.Contains(_researches.GetResearchData().CurrentResearch);

    private Research CurrentResearch => _researches.GetResearchData().CurrentResearch;
    public AI.Settler plantResearcher;
    public AI.Settler robotResearcher;
    public Transform plantInteractPosition;
    public Transform robotInteractPosition;
    private CommandTarget _commandTarget;

    private IResearchService _researches;

    protected override void OnNetworkPostSpawn() {
        base.OnNetworkPostSpawn();
        _researches = ServiceLocator.Container.Single<IResearchService>();
        _researches.RegisterResearchStation(this);
        _commandTarget = GetComponent<CommandTarget>();
    }

    public bool IsAnotherOnPosition(Race anotherResearcherRace) {
        if (anotherResearcherRace == Race.Plants && plantInteractPosition != null) {
            return plantResearcher.transform.position == plantInteractPosition.position;
        } else if (anotherResearcherRace == Race.Robots && robotInteractPosition != null) {
            return robotResearcher.transform.position == robotInteractPosition.position;
        }

        return false;
    }

    public void AddResearchPoints(int amount) {
        _researches.AddResearchPoints(amount);
        _progressData.ProgressData.Progress.Value = _researches.GetResearchData().ResearchProgress[CurrentResearch];
    }

    public void StartWorking() {
        _commandTarget.SetPerform(true);
    }

    public void StopWorking() {
        _commandTarget.SetPerform(true);
    }

    public void SetResearchData() {
        if (!HasResearch) {
            _progressData.ProgressData.Title = "не выбрано";
            _progressData.ProgressData.Progress.Value = 0;
            _progressData.ProgressData.Needed = 0;
            return;
        }

        var saveData = _researches.GetInitResearchData()[CurrentResearch];
        var data = _researches.GetResearchData();
        _progressData.ProgressData.Title = saveData.DisplayName;
        _progressData.ProgressData.Needed = saveData.Price;
        _progressData.ProgressData.Progress.Value = data.ResearchProgress[CurrentResearch];
    }
}