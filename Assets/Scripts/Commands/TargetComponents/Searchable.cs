using UnityEngine;
using System;

public class SearchableObj : MonoBehaviour, ISearchableObj {
	[Header("Searchable Settings")]
	public float requiredSearchTime = 3f;
	public bool canBeSearched = true;
	
	private float _searchProgress;
	private bool _isSearched;
	private bool _isBeingSearched;
	
	public bool IsSearched => _isSearched;
	public float SearchProgress => _searchProgress;
	public float RequiredSearchTime => requiredSearchTime;
	
	public bool CanBeSearched() {
		return canBeSearched && !_isSearched;
	}
	
	public void StartSearch() {
		if (!CanBeSearched()) {
			Debug.LogWarning($"Cannot start search on {gameObject.name}");
			return;
		}
		
		_isBeingSearched = true;
		_searchProgress = 0f;
		Debug.Log($"Started searching {gameObject.name}");
	}
	
	public void UpdateSearch(float amount) {
		if (_isSearched) return;
		
		_searchProgress += amount;
		
		if (_searchProgress >= requiredSearchTime) {
			_searchProgress = requiredSearchTime;
			_isSearched = true;
			_isBeingSearched = false;
			CompleteSearch();
		}
	}
	
	public void CompleteSearch() {
		_isBeingSearched = false;
		_isSearched = true;
		Debug.Log($"Search completed on {gameObject.name}. REWARD DROPPED!");
		OnSearchCompleted?.Invoke();
	}
	
	public void ResetSearch() {
		_isSearched = false;
		_isBeingSearched = false;
		_searchProgress = 0f;
	}
	
	public event Action OnSearchCompleted;
} 