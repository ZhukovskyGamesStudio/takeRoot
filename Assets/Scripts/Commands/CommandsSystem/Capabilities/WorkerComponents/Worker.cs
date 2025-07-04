using System;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.Serialization;

public class Worker : MonoBehaviour {
	public CommandType CommandCapabilities { get; private set; }
	
	public int CurrentCommandId = -1; // when no command = -1
	
	private IMovable _mover;
	private IDestroyer _destroyer;
	private ISearcher _searcher;
	private IWaterer _waterer;
	
	
	private void Start() {
		if (TryGetComponent(out _mover)) {
			AddCapability(CommandType.Move);
		}
		if (TryGetComponent(out _destroyer)) {
			AddCapability(CommandType.Destroy);
		}
		if (TryGetComponent(out _searcher)) {
			AddCapability(CommandType.Search);
		}
		if (TryGetComponent(out _waterer)) {
			AddCapability(CommandType.Water);
		}
		//_workerService.RegisterWorker(this);
	}
	
	public void AddCapability(CommandType capability) {
		CommandCapabilities |= capability;
	}

	public void RemoveCapability(CommandType capability) {
		CommandCapabilities &= ~capability;
	}
	
	public bool CanPerformNow(CommandType commandType) {
		return ((CommandCapabilities & commandType) == commandType) && CurrentCommandId == -1;
	}

	
	//Mover
	public bool TryMoveTo(Vector2 position) => _mover.TryMoveTo(position);
	public bool IsAtPosition(Vector2 position) => _mover.IsAtPosition(position);
	
	//Destroyer
	public void Hit(CommandTarget target) => _destroyer.Hit(target);

	//Searcher
	public void Search(CommandTarget target) => _searcher.Search(target);
	
	//Waterer
	public void Water(CommandTarget target) => _waterer.Water(target);
	
	public IMovable Mover => _mover;
	public IDestroyer Destroyer => _destroyer;
	public ISearcher Searcher => _searcher;
}