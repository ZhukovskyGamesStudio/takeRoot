using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Obsolete]
public class BuildingView : MonoBehaviour {
    [SerializeField]
    private GameObject _buildingToggle;

    [SerializeField]
    private GameObject _buildingPanel;

    [SerializeField]
    private List<GameObject> _buildingNotes;

    public void RedrawBuildingPanel(int availablePlans) {
        if (availablePlans == 0) {
            _buildingToggle.SetActive(false);
        } else {
            _buildingToggle.SetActive(true);
        }

        RedrawBuildingNotes(availablePlans);
    }

    private void RedrawBuildingNotes(int availablePlans) {
        for (int i = 0; i < availablePlans; i++) {
            TextMeshProUGUI text = _buildingNotes[i].GetComponentInChildren<TextMeshProUGUI>();
            Image icon = _buildingNotes[i].GetComponentInChildren<Image>();
            Button button = _buildingNotes[i].GetComponentInChildren<Button>();
            text.text = ObsoleteCoreEntryPoint.BuildingManager.GetBuildingPlanText(i);
            icon.sprite = ObsoleteCoreEntryPoint.BuildingManager.GetBuildingPlanIcon(i).sprite;
            int i1 = i;
            button.onClick.AddListener(delegate { ObsoleteCoreEntryPoint.BuildingManager.EnablePlan(i1); });
        }
    }
}