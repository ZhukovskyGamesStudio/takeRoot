using System;
using UnityEngine;

public class CommandTarget : MonoBehaviour {
	public CommandType CommandCapabilities { get; private set; }
	
	public int CurrentCommandId;
	
	private Health _health;
	private SearchableObj _searchable;
	
	private void Start() {
		if (TryGetComponent(out _health))
			AddCapability(CommandType.Destroy);
		if (TryGetComponent(out _searchable))
			AddCapability(CommandType.Search);
	}
	
	public void AddCapability(CommandType command) {
		CommandCapabilities |= command;
	}
	
	
	//Health
	public void TakeDamage(float damage) => _health?.TakeDamage(damage);
	public void Die() => _health?.Die();
	public bool IsDead => _health?.IsDead ?? false;
	
	//Search
	public void Search() => _searchable?.Search();
	public void EndSearch() => _searchable?.EndSearch();
	public bool Searched => _searchable?.Searched ?? false;
	
	
	public Health Health => _health;
	public SearchableObj Searchable => _searchable;
}