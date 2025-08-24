using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using UnityEngine;

public class CraftingStation : MonoBehaviour {
	public Dictionary<ResourceType, int> RequiredResources;

	private void Start() {
		ServiceLocator.Container.Single<ICraftingService>().AddCraftingStation(this);
		RequiredResources = new Dictionary<ResourceType, int>();
		RequiredResources.Add(ResourceType.Planks, 5);
	}

	public ResourceType GetRequiredResource() => RequiredResources.Keys.FirstOrDefault();
}