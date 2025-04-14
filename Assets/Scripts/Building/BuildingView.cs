using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingView : MonoBehaviour
{
    [SerializeField]private GameObject _buildingToggle;
    [SerializeField]private GameObject _buildingPanel;
    [SerializeField] private List<GameObject> _buildingNotes;
    public void RedrawBuildingPanel(int availablePlans)
    {
        if (availablePlans == 0)
            _buildingToggle.SetActive(false);
        else
            _buildingToggle.SetActive(true);
        RedrawBuildingNotes(availablePlans);
    }

    private void RedrawBuildingNotes(int availablePlans)
    {
        for (int i = 0; i < availablePlans; i++)
        {
            var text = _buildingNotes[i].GetComponentInChildren<TextMeshProUGUI>();
            var icon = _buildingNotes[i].GetComponentInChildren<Image>();
            var button = _buildingNotes[i].GetComponentInChildren<Button>();
            text.text = Core.BuildingManager.GetBuildingPlanText(i);
            icon.sprite = Core.BuildingManager.GetBuildingPlanIcon(i).sprite;
            var i1 = i;
            button.onClick.AddListener(delegate { Core.BuildingManager.EnablePlan(i1);});
        }
    }


}