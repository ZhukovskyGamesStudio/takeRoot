using System;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.Serialization;

public class CommandTarget : MonoBehaviour {
	public JobType JobCapabilities { get; private set; }

	public bool UseAnimatorWhenPerform = true;
	public Animator PerformingAnimator;
	
	public Transform InteractPosition;
	
	public int CurrentJobId = -1;

	public JobType CurrentJobType = JobType.None;
	public bool Reserved;
	
	private Health _health;
	private SearchableObj _searchable;
	private WaterLevel _waterLevel;
	private ICarriable _carriable;

	private void Start() {
		if (TryGetComponent(out _health))
			AddCapability(JobType.Destroy);
		if (TryGetComponent(out _searchable)){
			AddCapability(JobType.Search);
			_searchable.onSearched += () => RemoveCapability(JobType.Search);
		}
		if (TryGetComponent(out _waterLevel)) {
			AddCapability(JobType.Water);
		}
	}
	
	public void AddCapability(JobType job) {
		JobCapabilities |= job;
	}
	public void RemoveCapability(JobType job) {
		JobCapabilities &= ~job;
	}

	public bool CanPerform(JobType job) {
		return (JobCapabilities & job) == job && CurrentJobId == -1;
	}

	public void SetPerform(bool isPerforming) {
		if (PerformingAnimator == null || !UseAnimatorWhenPerform) return;
		if (isPerforming) {	
			PerformingAnimator.SetTrigger("Work");
		} else PerformingAnimator?.SetTrigger("Idle");
	}

	public void CancelJob() {
		ServiceLocator.Container.Single<ICommandService>().UnregisterJob(this); //TODO: cache service
	}
	
	//Health
	public void TakeDamage(float damage) => _health?.TakeDamage(damage);
	public bool IsDead => _health?.IsDead ?? false;
	public void Die() => _health.Die();
	
	//Search
	public void Search() => _searchable.Search();
	public void EndSearch() => _searchable.EndSearch();
	public bool Searched => _searchable.Searched;
	
	//Water
	public void Dry(float amount) => _waterLevel.Dry(amount);
	public void Water(float amount) => _waterLevel.Water(amount);
	public bool EnoughWater => _waterLevel.EnoughWater;
	
	//Carry
	public void TakeItem(Worker worker) => _carriable.TakeItem(worker);
	
	public Health Health => _health;
	public SearchableObj Searchable => _searchable;
}