using UnityEngine;
using UnityEngine.UI;

namespace Settlers.Test {
	public class TestCraftingStation : MonoBehaviour {
		public string Recipe;
		public CraftingStation craftingStation;

		public void Add() {
			craftingStation.AddRecipe(Recipe);
		}

		public void Remove() {
			craftingStation.RemoveRecipe(Recipe);
		}
		
	}
}