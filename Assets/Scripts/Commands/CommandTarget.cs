using System;
using UnityEngine;

public class CommandTarget : MonoBehaviour {
	public CommandType CommandCapabilities { get; private set; }
	
	private ICommand _currentCommand;
	
	private Health _health;
	private ISearchableObj _searchable;
	
	private void Start() {
		if (TryGetComponent(out _health))
			AddCapability(CommandType.Destroy);
		if (TryGetComponent(out _searchable))
			AddCapability(CommandType.Search);
	}
	
	public void AddCapability(CommandType command) {
		CommandCapabilities |= command;
	}
	
	public void RemoveCapability(CommandType command) {
		CommandCapabilities &= ~command;
	}

	public bool CanPerformNow(CommandType command) => 
		(CommandCapabilities & command) == command && _currentCommand == null;

	public void TakeCommand(ICommand command) {
		_currentCommand = command;
	}
	public void ReleaseCommand(ICommand command) {
		if (_currentCommand == command)
			_currentCommand = null;
	}
	
	//Health
	public void TakeDamage(float damage) => _health?.TakeDamage(damage);
	public void Die() => _health?.Die();
	public bool IsDead => _health?.IsDead ?? false;
	
	
	public Health Health => _health;
	public ISearchableObj Searchable => _searchable;
}