using UnityEngine;

public class Searcher : MonoBehaviour, ISearcher {
	[Header("Searcher Settings")]
	public float searchCooldown = 0.5f;
	private float _lastSearchTime;
	
	public void Search(CommandTarget target) {
		if (OnCooldown()) return;
		
		
		target.Search();
		_lastSearchTime = Time.time;
	}
	
	public bool OnCooldown() {
		return Time.time - _lastSearchTime < searchCooldown;
	}
} 