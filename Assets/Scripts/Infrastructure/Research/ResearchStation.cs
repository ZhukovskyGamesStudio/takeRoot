using System;
using System.Collections.Generic;
using CodeBase.Services;
using UnityEngine;


public class ResearchStation : MonoBehaviour {
	[SerializeField]private Progress _progressData;
	[SerializeField]private List<Research> _availableResearch;
	
	public bool HasResearch => _researches.GetResearchData().CurrentResearch != Research.None 
	                           && _availableResearch.Contains(_researches.GetResearchData().CurrentResearch);
	private Research CurrentResearch => _researches.GetResearchData().CurrentResearch;
	public AI.Settler plantResearcher;
	public AI.Settler robotResearcher;
	public Transform plantInteractPosition;
	public Transform robotInteractPosition;

	public bool IsAnotherOnPosition(Race anotherResearcherRace) {
		if (anotherResearcherRace == Race.Plants) {
			return plantResearcher.transform.position == plantInteractPosition.position;
		} else if (anotherResearcherRace == Race.Robots) {
			return robotResearcher.transform.position == robotInteractPosition.position;
		}
		return false;
	}
	
	private IResearchService _researches;
	private void Start() {
		_researches = ServiceLocator.Container.Single<IResearchService>();
		_researches.RegisterResearchStation(this);
	}

	public void DoResearch(int amount) {
		_researches.AddResearchPoints(amount);
		_progressData.ProgressData.Progress.Value = _researches.GetResearchData().ResearchProgress[CurrentResearch];
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

	private void OnDestroy() {
		//_researches.UnregisterResearchStation(this);
	}
}