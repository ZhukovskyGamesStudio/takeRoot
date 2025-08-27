using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchPanelView : MonoBehaviour {
    [SerializeField]
    private List<ResearchView> _researches;

    [SerializeField]
    private TextMeshProUGUI _titleText, _descriptionText, _requirementsText, _progressText, _selectedText;

    [SerializeField]
    private List<LayoutGroup> _layoutGroups;

    [SerializeField]
    private GameObject _selectedPanel;

    [SerializeField]
    private Slider _progressSlider;

    [SerializeField]
    private Button _researchButton;
    
    private ResearchSaveData _saveData;
    private ResearchViewPresenter _presenter;

    private ResearchData _selectedResearch;
    
    public void InitData(Dictionary<Research, ResearchData> researches, ResearchSaveData saveData, ResearchViewPresenter presenter) {
        _saveData = saveData;
        _presenter = presenter;
        
        foreach (ResearchView research in _researches) {
            research.InitData(researches[research.Id], this);
        }
    }

    private void FixedUpdate() {
        UpdateData();
    }

    private void UpdateData() {
        foreach (ResearchView research in _researches) {
            research.UpdateData(_saveData.ResearchProgress[research.Id], _saveData.CurrentResearch == research.Id);
        }

        if (_selectedResearch != null) {
            bool researchCompleted = _saveData.ResearchProgress[_selectedResearch.Id] == _selectedResearch.Price;
            _researchButton.interactable = !researchCompleted && _selectedResearch.Researchable;
        }
        
        if (_saveData.CurrentResearch != Research.None) UpdateCurrentResearchData();
        else {
            _progressSlider.gameObject.SetActive(false);
            _selectedText.text = "Не выбрана";
        }
    }

    private void UpdateCurrentResearchData() {
        _progressSlider.gameObject.SetActive(true);
        ResearchData selectedResearchData = _presenter.GetResearchData(_saveData.CurrentResearch);
        
        int progress = _saveData.ResearchProgress[_saveData.CurrentResearch];
        int price = selectedResearchData.Price;
        
        _progressText.text = $"{progress}/{price}";
        _progressSlider.value = (float)progress / price;

        _selectedText.text = selectedResearchData.DisplayName;
    }

    public void SelectResearch(ResearchData research) {
        _selectedResearch = research;
        _selectedPanel.SetActive(true);

        _titleText.text = research.DisplayName;
        _descriptionText.text = research.Description;

        _requirementsText.text = string.Empty;
        if (research.Requirements.Count == 0) {
            _requirementsText.text = "Нет";
        }
        foreach (Research req in research.Requirements) {
            _requirementsText.text += $"{_presenter.GetResearchData(req).DisplayName}\n";
        }
        
        UpdateLayoutGroups();
    }

    public void UnselectResearch() {
        _selectedResearch = null;
        _selectedPanel.SetActive(false);
    }
    
    public void StartResearch() {
        if (_selectedResearch == null) return;
        
        _presenter.SelectResearch(_selectedResearch.Id);
        _presenter.AddPoints();
    }

    private void UpdateLayoutGroups() {
        foreach (LayoutGroup group in _layoutGroups) {
            LayoutRebuilder.ForceRebuildLayoutImmediate(group.transform as RectTransform);
        }
    }
}