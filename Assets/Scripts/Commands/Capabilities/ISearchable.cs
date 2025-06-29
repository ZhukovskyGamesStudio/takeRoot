using UnityEngine;

public interface ISearchableObj {
	bool IsSearched { get; }
	float SearchProgress { get; }
	float RequiredSearchTime { get; }
	void StartSearch();
	void UpdateSearch(float deltaTime);
	void CompleteSearch();
	bool CanBeSearched();
} 