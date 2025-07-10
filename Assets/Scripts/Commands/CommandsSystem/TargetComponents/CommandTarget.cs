using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CommandTarget : MonoBehaviour {
	public CommandType CommandCapabilities { get; private set; }

	public bool UseAnimatorWhenPerform = true;
	public Animator PerformingAnimator;
	
	public Transform InteractPosition;
	
	public int CurrentCommandId = -1;
	
	private Health _health;
	private SearchableObj _searchable;
	private WaterLevel _waterLevel;
	private ICarriable _carriable;

	private void Start() {
		if (TryGetComponent(out _health))
			AddCapability(CommandType.Destroy);
		if (TryGetComponent(out _searchable)){
			AddCapability(CommandType.Search);
			_searchable.onSearched += () => RemoveCapability(CommandType.Search);
		}
		if (TryGetComponent(out _waterLevel)) {
			AddCapability(CommandType.Water);
		}
		if (TryGetComponent(out _carriable)) {
			AddCapability(CommandType.Carry);
		}
	}
	
	public void AddCapability(CommandType command) {
		CommandCapabilities |= command;
	}
	public void RemoveCapability(CommandType command) {
		CommandCapabilities &= ~command;
	}

	public bool CanPerform(CommandType command) {
		return (CommandCapabilities & command) == command && CurrentCommandId == -1;
	}

	public void SetPerform(bool isPerforming) {
		if (PerformingAnimator == null || !UseAnimatorWhenPerform) return;
		if (isPerforming) {	
			PerformingAnimator.SetTrigger("Work");
		} else PerformingAnimator?.SetTrigger("Idle");
	}
	
	//Health
	public void TakeDamage(float damage) => _health?.TakeDamage(damage);
	public bool IsDead => _health?.IsDead ?? false;
	
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