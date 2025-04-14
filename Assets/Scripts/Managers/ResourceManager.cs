using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    [SerializeField]
    private ResourcesTable _resourcesTable;

    [SerializeField]
    private Transform _resourcesHolder;

    public SerializedDictionary<ResourceType, Sprite> EquipmentIcons = new SerializedDictionary<ResourceType, Sprite>();

    private Dictionary<Vector2Int, ResourceView> _scatteredResources = new Dictionary<Vector2Int, ResourceView>();

    public Transform ResourcesHolder => _resourcesHolder;

    private void Awake() {
        Core.ResourceManager = this;
    }

    public static ResourceData FindAllAvailableResources(ResourceType type)
    {
        var resourceOnGround = FindFitResourcesOnGround(type);
        var storages = FindFitStorages(type);
        int totalAmount = 0;
        totalAmount += resourceOnGround.Sum(r => r.Amount);
        totalAmount += storages.Sum(s => s.Resource.Amount);
        return new ResourceData()
        {
            ResourceType = type,
            Amount = totalAmount
        };
    }

    private static ResourceView SpawnResource(ResourceType resourceType, Vector2Int cell) {
        var prefab = Core.ResourceManager._resourcesTable.ResourceViewPrefabs.First(s => s.ResourceType == resourceType);
        //TODO add pool
        ResourceView r = Instantiate(prefab, Core.ResourceManager._resourcesHolder);
        //Core.ResourceManager._scatteredResources.Add(cell, r);
        return r;
    }

    public static ResouseUiView SpawnResourceUi(ResourceType resourceType) {
        var prefab = Core.ResourceManager._resourcesTable.ResourceUiViewPrefabs.First(s => s.ResourceType == resourceType);
        //TODO add pool
        ResouseUiView r = Instantiate(prefab);
        return r;
    }

    public static ResourceView SpawnResourceAt(ResourceData resource, Vector2Int at) {
        ResourceView resourceView = SpawnResource(resource.ResourceType, at);
        resourceView.transform.position = new Vector3(at.x, at.y);
        resourceView.SetAmount(resource.Amount);
        return resourceView;
    }

    public static void ClearResourceView(ResourceView resource)
    { 
        if (Core.ResourceManager._scatteredResources.TryGetValue(resource.Interactable.GetInteractableCell, out _))
            Core.ResourceManager._scatteredResources.Remove(resource.Interactable.GetInteractableCell);
    }
    
    public static List<ResourceView> SpawnResourcesAround(List<ResourceData> resources, Vector2Int centerCell) {
        const int maxResourceInCell = 10;
        List<ResourceView> spawnedResources = new List<ResourceView>();
        int checkedN = 0;
        foreach (ResourceData resourceData in resources) {
            int remainingAmount = resourceData.Amount;

            while (remainingAmount > 0) {
                Vector2Int targetCell = centerCell + GetSpiralOffset(checkedN);

                // Check if the cell is occupied by a different resource
                if (Core.ResourceManager._scatteredResources.TryGetValue(targetCell, out ResourceView existingResource)) {
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
                    Core.ResourceManager._scatteredResources[targetCell] = newResource;

                    remainingAmount -= toSpawn;
                }

                checkedN++;
                // Break out of the loop once all resources are spawned
                if (remainingAmount <= 0) break;
            }
        }

        return spawnedResources;
    }

    public static List<ResourceView> SpawnResourcesAround(List<ResourceData> resources, List<Vector2Int> centerCells)
    {
        const int maxResourceInCell = 10;
        List<ResourceView> spawnedResources = new List<ResourceView>();
        int checkedN = 0;
        foreach (ResourceData resourceData in resources)
        {
            int remainingAmount = resourceData.Amount;

            while (remainingAmount > 0)
            {
                foreach (Vector2Int centerCell in centerCells)
                {
                    Vector2Int targetCell = centerCell + GetSpiralOffset(checkedN);

                    if (centerCells.Contains(targetCell))
                    {
                        checkedN++;
                        continue;
                    }

                    // Check if the cell is occupied by a different resource
                    if (Core.ResourceManager._scatteredResources.TryGetValue(targetCell, out ResourceView existingResource))
                    {
                        if (existingResource.ResourceType == resourceData.ResourceType)
                        {
                            // Add to the existing resource if it doesn't exceed the limit
                            var availableSpace = maxResourceInCell - existingResource.Amount;
                            if (availableSpace > 0)
                            {
                                var toAdd = Mathf.Min(availableSpace, remainingAmount);
                                existingResource.SetAmount(existingResource.Amount + toAdd);
                                remainingAmount -= toAdd;
                            }
                        }
                    }
                    else
                    {
                        // Create a new resource in the cell
                        int toSpawn = Mathf.Min(remainingAmount, maxResourceInCell);
                        ResourceView newResource = SpawnResource(resourceData.ResourceType, targetCell);
                        newResource.transform.position = new Vector3(targetCell.x, targetCell.y);
                        newResource.SetAmount(toSpawn);

                        spawnedResources.Add(newResource);
                        Core.ResourceManager._scatteredResources[targetCell] = newResource;

                        remainingAmount -= toSpawn;
                    }

                    checkedN++;
                    // Break out of the loop once all resources are spawned
                    if (remainingAmount <= 0) break;
                }
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

    public Storagable FindEmptyStorageForResorce(ResourceData resource) {
        Storagable[] storages = FindObjectsByType<Storagable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        //TODO если ресурс целиком не влезает в стол или нет столов - всё ломается
        //TODO стол выбирается случайно, а не ближайший
        Storagable fittingStorage = storages.Where(s => s.CanStore(resource)).OrderByDescending(s => s.CanStore(resource)).FirstOrDefault();
        Debug.Log(fittingStorage);
        return fittingStorage;
    }

    public Storagable FindClosestAvailableStorage(ResourceData resource, Vector2Int from) {
        Storagable[] storages = FindObjectsByType<Storagable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        if (storages.Length == 0) return null;
        List<Storagable> fittingStorage = storages.Where(s => s.CanStore(resource)).ToList();
        if (fittingStorage.Count == 0) return null;
        float shortestPath = Mathf.Infinity;
        Storagable closestStorage = null;
        for (int i = 0; i < fittingStorage.Count; i++) {
            if (i == 10) break;
            var pathLength = Core.AStarPathfinding
                .FindPath(from, fittingStorage[i].GetComponent<Interactable>().InteractableCells, out bool isPathExist).Count;
            if (!isPathExist) {
                continue;
            }

            if (pathLength < shortestPath) {
                shortestPath = pathLength;
                closestStorage = fittingStorage[i];
            }
        }

        //TODO find closest
        return closestStorage;
    }

    public static List<ResourceView> FindFitResourcesOnGround(ResourceType type) {
        ResourceView[] resourcesOnGround = FindObjectsByType<ResourceView>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        return resourcesOnGround
            .Where(r => r.ResourceType == type && r.Amount != 0 && r.GetEcsComponent<Interactable>().CommandToExecute == null).ToList();
    }

    public static List<Storagable> FindFitStorages(ResourceType type) {
        Storagable[] storages = FindObjectsByType<Storagable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        return storages.Where(r =>
            r.Resource.ResourceType == type && r.Resource.Amount != 0 && r.GetComponent<Interactable>().CommandToExecute == null).ToList();
    }
}

[Serializable]
public class ResourceData {
    public ResourceType ResourceType;
    public int Amount = 1;

    public static ResourceData Empty => new() {
        ResourceType = ResourceType.None,
        Amount = 0
    };
}