using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawnPoint : MonoBehaviour
{
	private const int maxResourceInCell = 10;
	
	[SerializeField]private List<ResourceData> ResourcesToSpawn;
	
	private IGameFactory _factory;
	private IDataProvider _data;
	

	public void Init(IGameFactory factory, IDataProvider data) {
		_factory = factory;
		_data = data;
	}

	public void AddResourceToSpawn(ResourceData resource) {
		var existingResource = ResourcesToSpawn.Find(r => r.ResourceType == resource.ResourceType);
		if (existingResource != null) {
			existingResource.Amount += resource.Amount;
		}
		else {
			ResourcesToSpawn.Add(resource);
		}
	}
	
	public void SpawnResources() {
		SpawnResourceAround();
		
	}
	
	private void SpawnResourceAround() {
		Vector2Int center2Int = new Vector2Int((int)transform.position.x, (int)transform.position.z);
		int checkedN = 0;
		foreach (ResourceData resource in ResourcesToSpawn) {
			int remainingAmount = resource.Amount;
			while (remainingAmount > 0) {
				Vector2Int targetCell = center2Int + GetSpiralOffset(checkedN);
				
				if (_data.WorldResourcesData.ResourcesOnScene.TryGetValue(targetCell,
					    out ResourceView existingResource)) {
					if (existingResource.ResourceType == resource.ResourceType) {
						var availableSpace = maxResourceInCell - existingResource.Amount;
						if (availableSpace > 0) {
							var toAdd = Mathf.Min(availableSpace, remainingAmount);
							existingResource.SetAmount(existingResource.Amount + toAdd);
							remainingAmount -= toAdd;
						}
					}
				} else {
					int toSpawn = Mathf.Min(remainingAmount, maxResourceInCell);
					_factory.CreateResource(resource.ResourceType.ToString(), new Vector3(targetCell.x, targetCell.y), toSpawn);

					remainingAmount -= toSpawn;
				}

				checkedN++;
				if (remainingAmount <= 0) break;
					
			}
		}
	}

	
	private Vector2Int GetSpiralOffset(int n) {
		// Directions: right, up, left, down
		var directions = new[] {
			new Vector2Int(1, 0), // Right
			new Vector2Int(0, 1), // Up
			new Vector2Int(-1, 0), // Left
			new Vector2Int(0, -1) // Down
		};

		int x = 0, y = 0; // Start at the center
		int stepSize = 1; // Initial step size
		int directionIndex = 0; // Start moving right
		int stepsTakenInCurrentDirection = 0;
		int stepsRemaining = stepSize;

		for (int i = 0; i <= n; i++) {
			// Move in the current direction
			x += directions[directionIndex].x;
			y += directions[directionIndex].y;

			stepsTakenInCurrentDirection++;
			stepsRemaining--;

			// Change direction when steps in the current direction are exhausted
			if (stepsRemaining == 0) {
				directionIndex = (directionIndex + 1) % 4; // Cycle through directions
				stepsTakenInCurrentDirection = 0;

				// Increase step size every two direction changes
				if (directionIndex == 0 || directionIndex == 2) {
					stepSize++;
				}

				stepsRemaining = stepSize; // Reset steps for the new direction
			}
		}

		return new Vector2Int(x, y);
	}
		
}