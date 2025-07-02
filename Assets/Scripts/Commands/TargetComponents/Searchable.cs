using UnityEngine;
using System;

public class SearchableObj : MonoBehaviour {
	[Header("Searchable Settings")]
	public int requiredSearchPoints = 5;
	public int currentResearchPoints;
	
	
	public event Action onSearched;
	
	public bool Searched => currentResearchPoints >= requiredSearchPoints;

	public void Search() {
		if (Searched) return;
		
		currentResearchPoints++;
	}

	public void EndSearch() {
		onSearched?.Invoke();
	}
	
	
} 