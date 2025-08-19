using System;
using UnityEngine;

public class Potato : MonoBehaviour{
	public GameObject plantObj;

	private void Start() {
		GetComponent<SearchableObj>().onSearched += DeletePotato;
	}
	private void DeletePotato() {
		plantObj.SetActive(false);
	}
}