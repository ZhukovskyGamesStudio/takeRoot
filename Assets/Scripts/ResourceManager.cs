using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public static ResourceManager Instance;

    [SerializeField]
    private ResourcesTable _resourcesTable;

    private Dictionary<Vector2Int, ResourceView> _scatteredResources = new Dictionary<Vector2Int, ResourceView>();

    [SerializeField]
    private Transform _resourcesHolder;

    private void Awake() {
        Instance = this;
    }

    private static ResourceView SpawnResource(ResourceType resourceType, Vector2Int cell) {
        var prefab = Instance._resourcesTable.ResourceViewPrefabs.First(s => s.ResourceType == resourceType);
        //TODO add pool
        ResourceView r = Instantiate(prefab, Instance._resourcesHolder);
        Instance._scatteredResources.Add(cell, r);
        return r;
    }

    public static ResouseUiView SpawnResourceUi(ResourceType resourceType) {
        var prefab = Instance._resourcesTable.ResourceUiViewPrefabs.First(s => s.ResourceType == resourceType);
        //TODO add pool
        ResouseUiView r = Instantiate(prefab);
        return r;
    }

    public static List<ResourceView> SpawnResourcesAround(List<ResorceData> resources, Vector2Int centerCell) {
        const int maxResourceInCell = 10;
        List<ResourceView> spawnedResources = new List<ResourceView>();
        int checkedN = 0;
        foreach (ResorceData resourceData in resources) {
            int remainingAmount = resourceData.Amount;

            while (remainingAmount > 0) {
                Vector2Int targetCell = centerCell + GetSpiralOffset(checkedN);

                // Check if the cell is occupied by a different resource
                if (Instance._scatteredResources.TryGetValue(targetCell, out ResourceView existingResource)) {
                    if (existingResource.ResourceType == resourceData.ResourceType) {
                        // Add to the existing resource if it doesn't exceed the limit
                        var availableSpace = maxResourceInCell - existingResource.Amount;
                        if (availableSpace > 0) {
                            var toAdd = Mathf.Min(availableSpace, remainingAmount);
                            existingResource.SetAmount(existingResource.Amount + toAdd);
                            remainingAmount -= toAdd;
                        }
                    }
                } else {
                    // Create a new resource in the cell
                    int toSpawn = Mathf.Min(remainingAmount, maxResourceInCell);
                    ResourceView newResource = SpawnResource(resourceData.ResourceType, targetCell);
                    newResource.transform.position = new Vector3(targetCell.x, targetCell.y);
                    newResource.SetAmount(toSpawn);

                    spawnedResources.Add(newResource);
                    Instance._scatteredResources[targetCell] = newResource;

                    remainingAmount -= toSpawn;
                }

                checkedN++;
                // Break out of the loop once all resources are spawned
                if (remainingAmount <= 0) break;
            }
        }

        return spawnedResources;
    }

    public static Vector2Int GetSpiralOffset(int n) {
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

    public Table FindEmptyStorageForResorce(ResorceData resource) {
        Table[] storages = FindObjectsByType<Table>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        //TODO если ресурс целиком не влезает в стол или нет столов - всё ломается
        //TODO стол выбирается случайно, а не ближайший
        Table fittingStorage = storages.Where(s=>s.IsStorageActive && s.ResorceStorage.CanFitResource(resource) >= resource.Amount). OrderByDescending(s => s.ResorceStorage.CanFitResource(resource))
            .FirstOrDefault();
        return fittingStorage;
    }
}

[Serializable]
public class ResorceData {
    public ResourceType ResourceType;
    public int Amount;
}