using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager : MonoBehaviour {
    public static BuildingManager Instance;

    [SerializeField]
    private BuildingView _buildingView;

    private int _availablePlans = 2;

    private BuildingPlan _currentPlan;

    private List<BuildingPlan> _plans;

    private void Awake() {
        Instance = this;
        LoadPlans();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse1) && _currentPlan != null)
            CancelPlan();
    }

    public void AddNewPlan() {
        _availablePlans++;
        _buildingView.RedrawBuildingPanel(_availablePlans);
    }

    public void EnablePlan(int index) {
        if (_currentPlan != null)
            CancelPlan();
        _currentPlan = Instantiate(_plans[index]);
    }

    private void CancelPlan() {
        Destroy(_currentPlan.gameObject);
        _currentPlan = null;
    }

    public void OnPlanPlaced() {
        _currentPlan = null;
    }

    private void LoadPlans() {
        _plans = Resources.LoadAll<BuildingPlan>("BuildingPlans").ToList();
        _buildingView.RedrawBuildingPanel(_availablePlans);
    }

    public string GetBuildingPlanText(int index) {
        return _plans[index].Name;
    }

    public SpriteRenderer GetBuildingPlanIcon(int index) {
        return _plans[index].Icon;
    }
}