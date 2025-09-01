using System.Collections.Generic;
using CodeBase.Services;
using Settlers.Test;
using UnityEngine;

public class CommandTarget : MonoBehaviour {
    public CommandTargetData Data;
    
    [HideInInspector]
    public JobType JobCapabilities { get; private set; }

    public PlannedJobView PlannedJob;
    public bool UseAnimatorWhenPerform = true;
    public Animator PerformingAnimator;

    public Transform InteractPosition;
    public List<Vector3> InteractPositions => _gridObject.GetFreeNeighbors();
    
    [HideInInspector]
    public int CurrentJobId = -1;

    //public JobType CurrentJobType = JobType.None;
    [HideInInspector]
    public bool Reserved;

    private Health _health;
    private SearchableObj _searchable;
    private WaterLevel _waterLevel;
    private GridObject _gridObject;

    private void Start() {
        if (TryGetComponent(out _health)) {
            AddCapability(JobType.Destroy);
        }

        if (TryGetComponent(out _searchable)) {
            AddCapability(JobType.Search);
            _searchable.onSearched += () => RemoveCapability(JobType.Search);
        }

        if (TryGetComponent(out _waterLevel)) {
            AddCapability(JobType.Water);
        }
        _gridObject = GetComponent<GridObject>();
    }

    public void AddCapability(JobType job) {
        JobCapabilities |= job;
        Data.JobCapabilities |= job;
    }

    public void RemoveCapability(JobType job) {
        JobCapabilities &= ~job;
        Data.JobCapabilities &= ~job;
    }

    public bool CanPerform(JobType job) {
        return (JobCapabilities & job) == job && CurrentJobId == -1;
    }

    public void SetPerform(bool isPerforming) {
        if (PerformingAnimator == null || !UseAnimatorWhenPerform) {
            return;
        }

        if (isPerforming) {
            PerformingAnimator.SetTrigger("Work");
        } else {
            PerformingAnimator?.SetTrigger("Idle");
        }
    }

    public void TrySetJob(JobType job) {
        if (Data.HasJob) {
            return;
        }

        if ((Data.JobCapabilities & job) != job) {
            return; //TODO: update capabilities change
        }

        Data.CurrentJob = job;
        PlannedJob.Enable(job);
        ServiceLocator.Container.Single<ICommandService>().RegisterJob(Data.Id, this);
    }

    public void CancelJob() {
        Reserved = false;
        Data.CurrentJob = JobType.None;
        Data.AssignedSettler = null;
        PlannedJob.gameObject.SetActive(false);
        ServiceLocator.Container.Single<ICommandService>().UnregisterJob(Data.Id); //TODO: cache service
    }

    //Health
    public void TakeDamage(float damage) {
        _health?.TakeDamage(damage);
    }

    public bool IsDead => _health?.IsDead ?? false;

    public void Die() {
        _health.Die();
    }

    //Search
    public void Search() {
        _searchable.Search();
    }

    public void EndSearch() {
        _searchable.EndSearch();
    }

    public bool Searched => _searchable.Searched;

    //Water
    public void Dry(float amount) {
        _waterLevel.Dry(amount);
    }

    public void Water(float amount) {
        _waterLevel.ChangeWater(amount);
    }

    public bool EnoughWater => _waterLevel.EnoughWater;

    //Carry

    public Health Health => _health;
    public SearchableObj Searchable => _searchable;
}