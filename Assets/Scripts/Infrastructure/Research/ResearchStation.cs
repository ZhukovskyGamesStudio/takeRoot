using System;
using System.Collections.Generic;
using CodeBase.Services;
using UnityEngine;


public class ResearchStation : MonoBehaviour {
	[SerializeField]private Progress _progressData;
	[SerializeField] private List<Research> _availableResearch;
	
	public bool HasResearch => _researches.GetResearchData().CurrentResearch != Research.None 
	                           && _availableResearch.Contains(_researches.GetResearchData().CurrentResearch);
	private Research CurrentResearch => _researches.GetResearchData().CurrentResearch;
	
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
		_progressData.ProgressData.Title = _researches.GetInitResearchData()[CurrentResearch].DisplayName;
		_progressData.ProgressData.Needed = _researches.GetInitResearchData()[CurrentResearch].Price;
		_progressData.ProgressData.Progress.Value = _researches.GetResearchData().ResearchProgress[CurrentResearch];
	}

	private void OnDestroy() {
		//_researches.UnregisterResearchStation(this);
	}
}