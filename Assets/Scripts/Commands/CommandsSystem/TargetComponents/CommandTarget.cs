using System;
using UnityEngine;

public class CommandTarget : MonoBehaviour {
	public CommandType CommandCapabilities { get; private set; }

	public Transform InteractPosition;
	
	public int CurrentCommandId;
	
	private Health _health;
	private SearchableObj _searchable;
	private WaterLevel _waterLevel;

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
	}
	
	public void AddCapability(CommandType command) {
		CommandCapabilities |= command;
	}
	public void RemoveCapability(CommandType command) {
		CommandCapabilities &= ~command;
	}
	
	
	//Health
	public void TakeDamage(float damage) => _health?.TakeDamage(damage);
	public bool IsDead => _health?.IsDead ?? false;
	
	//Search
	public void Search() => _searchable?.Search();
	public void EndSearch() => _searchable?.EndSearch();
	public bool Searched => _searchable?.Searched ?? false;
	
	//Water
	public void Dry(float amount) => _waterLevel.Dry(amount);
	public void Water(float amount) => _waterLevel.Water(amount);
	public bool EnoughWater => _waterLevel.EnoughWater;
	
	public Health Health => _health;
	public SearchableObj Searchable => _searchable;
}