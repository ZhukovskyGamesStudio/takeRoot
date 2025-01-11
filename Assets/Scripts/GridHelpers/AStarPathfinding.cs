using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class AStarPathfinding : MonoBehaviour {
    public static AStarPathfinding Instance;

    public HashSet<Vector2Int> obstaclePositions = new HashSet<Vector2Int>(); // List of obstacles in the form of grid positions

    private Dictionary<Vector2Int, Node> grid;
    private List<Node> openList = new List<Node>();
    private HashSet<Node> closedList = new HashSet<Node>();

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        StartCoroutine(WaitAndInit());
    }

    private IEnumerator WaitAndInit() {
        yield return new WaitForEndOfFrame();
        InitializeGrid();
        FindObstacles();
        InvokeRepeating(nameof(UpdateWalkable), 0, 1);
    }

    // Initialize the grid with walkable and blocked nodes
    private void InitializeGrid() {
        grid = new Dictionary<Vector2Int, Node>();
        Rect rect = GridManager.Instance.GridSize;
        Vector2Int min = new Vector2Int((int)rect.x, (int)rect.y);
        Vector2Int max = new Vector2Int((int)rect.width, (int)rect.height);
        for (int x = min.x; x < max.x; x++) {
            for (int y = min.x; y < max.y; y++) {
                Vector2Int position = new Vector2Int(x, y);
                grid.Add(position, new Node(position, true));
            }
        }
    }

    private void FindObstacles() {
        obstaclePositions.Clear();
        Gridable[] r = FindObjectsByType<Gridable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (Gridable gridable in r) {
            obstaclePositions. AddRange(gridable.GetOccupiedPositions());
        }
    }

    private void UpdateWalkable() {
        foreach (Vector2Int key in grid.Keys) {
            grid[key].walkable = true; // Assume walkable by default
        }

        foreach (Vector2Int obstacle in obstaclePositions.Where(obstacle => grid.ContainsKey(obstacle))) {
            grid[obstacle].walkable = false;
        }
    }

    // The A* pathfinding method
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end) {
        Node startNode = grid[start];
        Node targetNode = grid[end];

        openList.Clear();
        closedList.Clear();

        openList.Add(startNode);

        while (openList.Count > 0) {
            // Get the node with the lowest fCost
            Node currentNode = GetNodeWithLowestFCost(openList);
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // If we reach the target, reconstruct the path
            if (currentNode.position == targetNode.position) {
                return RetracePath(startNode, currentNode);
            }

            // Evaluate each of the neighbors
            foreach (Node neighbor in GetNeighbors(currentNode)) {
                if (!neighbor.walkable || closedList.Contains(neighbor))
                    continue;

                int newGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newGCost < neighbor.gCost || !openList.Contains(neighbor)) {
                    neighbor.gCost = newGCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return new List<Vector2Int>(); // Return an empty path if no path is found
    }

    // Get the node with the lowest fCost from the open list
    private Node GetNodeWithLowestFCost(List<Node> list) {
        Node lowestFCostNode = list[0];
        foreach (Node node in list) {
            if (node.fCost < lowestFCostNode.fCost)
                lowestFCostNode = node;
        }

        return lowestFCostNode;
    }

    // Get the neighbors of a node (up, down, left, right)
    private List<Node> GetNeighbors(Node node) {
        List<Node> neighbors = new List<Node>();

        Vector2Int[] directions = new Vector2Int[] {
            new Vector2Int(0, 1), // Up
            new Vector2Int(1, 0), // Right
            new Vector2Int(0, -1), // Down
            new Vector2Int(-1, 0), // Left
        };

        foreach (Vector2Int direction in directions) {
            Vector2Int neighborPos = node.position + direction;
            if (IsValidPosition(neighborPos)) {
                neighbors.Add(grid[neighborPos]);
            }
        }

        return neighbors;
    }

    // Check if a position is within the bounds of the grid
    private bool IsValidPosition(Vector2Int position) {
        return grid.ContainsKey(position);
    }

    // Get the Manhattan distance (heuristic) between two nodes
    private int GetDistance(Node a, Node b) {
        return Mathf.Abs(a.position.x - b.position.x) + Mathf.Abs(a.position.y - b.position.y);
    }

    // Retrace the path from the target to the start
    private List<Vector2Int> RetracePath(Node startNode, Node endNode) {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
}