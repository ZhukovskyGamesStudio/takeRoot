using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    [SerializeField] private BuildingView _buildingView;
    [SerializeField]private List<BuildingPlan> _plans;
    private int availablePlans = 2;

    private BuildingPlan _currentPlan;
    private void Awake()
    {
        Instance = this;
        LoadPlans();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && _currentPlan != null) 
            CancelPlan();
    }

    public void AddNewPlan()
    {
        availablePlans++;
        _buildingView.RedrawBuildingPanel(availablePlans);
    }
    public void EnablePlan(int index)
    {
        if (_currentPlan != null)
            CancelPlan();
        _currentPlan = Instantiate(_plans[index]);
    }

    private void CancelPlan()
    {
        Destroy(_currentPlan.gameObject);
        _currentPlan = null;
    }

    public void OnPlanPlaced()
    {
        _currentPlan = null;
    }
    private void LoadPlans()
    {
        var plans = Resources.LoadAll<BuildingPlan>("BuildingPlans");
        foreach (BuildingPlan plan in plans)
        {
            _plans.Add(plan);
        }
        
        _buildingView.RedrawBuildingPanel(availablePlans);
    }

    public string GetBuildingPlanText(int index)
    {
        return _plans[index].Name;
    }

    public SpriteRenderer GetBuildingPlanIcon(int index)
    {
        return _plans[index].Icon;
    }
    
}