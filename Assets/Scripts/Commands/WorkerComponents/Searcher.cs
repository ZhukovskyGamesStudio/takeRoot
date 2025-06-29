using UnityEngine;

public class Searcher : MonoBehaviour, ISearcher {
	[Header("Searcher Settings")]
	public float searchPower = 0.25f; // Сколько прогресса добавляется за тик
	public float searchCooldown = 0.5f;
	
	private float _lastSearchTime;
	private CommandTarget _currentTarget;
	private ISearchableObj _currentSearchable;
	
	public void Search(CommandTarget target) {
		if (!CanSearch()) return;
		
		if (target == null) {
			Debug.LogWarning("Cannot search null target");
			return;
		}
		
		var searchable = target.GetComponent<ISearchableObj>();
		if (searchable == null) {
			Debug.LogWarning($"Target {target.name} is not searchable");
			return;
		}
		
		if (!searchable.CanBeSearched()) {
			Debug.LogWarning($"Target {target.name} cannot be searched right now");
			return;
		}
		
		searchable.UpdateSearch(searchPower);
		_lastSearchTime = Time.time;
		Debug.Log($"Searched {target.name} for {searchPower}, progress: {searchable.SearchProgress}/{searchable.RequiredSearchTime}");
		
		if (searchable.IsSearched) {
			searchable.CompleteSearch();
			Debug.Log($"Search completed for {target.name}");
		}
	}
	
	public bool CanSearch() {
		return Time.time - _lastSearchTime >= searchCooldown;
	}
	
	public float GetSearchCooldownRemaining() {
		float timeSinceLastSearch = Time.time - _lastSearchTime;
		return Mathf.Max(0f, searchCooldown - timeSinceLastSearch);
	}
	
	public void SetSearchSpeed(float speed) {
		this.searchPower = speed;
	}
	
	public void SetSearchCooldown(float cooldown) {
		this.searchCooldown = cooldown;
	}
	
	private void Update() {
		if (_currentSearchable != null && !_currentSearchable.IsSearched) {
			_currentSearchable.UpdateSearch(searchPower * Time.deltaTime);
			
			if (_currentSearchable.IsSearched) {
				_currentSearchable.CompleteSearch();
				_currentSearchable = null;
				_currentTarget = null;
				Debug.Log("Search completed");
			}
		}
	}
} 