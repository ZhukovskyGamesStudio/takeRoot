using System;
using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinding : MonoBehaviour {
    public static AStarPathfinding Instance;
    public Vector2Int gridSize = new Vector2Int(10, 10);  // Example grid size
    public List<Vector2Int> obstaclePositions = new List<Vector2Int>();  // List of obstacles in the form of grid positions

    private Node[,] grid;
    private List<Node> openList = new List<Node>();
    private HashSet<Node> closedList = new HashSet<Node>();

    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
        
        InitializeGrid();
        FindObstacles();
        UpdateWalkable();
    }

    // Initialize the grid with walkable and blocked nodes
    private void InitializeGrid()
    {
        grid = new Node[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                bool isWalkable = !obstaclePositions.Contains(position);  // Mark the obstacle positions as non-walkable
                grid[x, y] = new Node(position, isWalkable);
            }
        }
    }

    private void FindObstacles() {
        obstaclePositions.Clear();
        InteractableObject[] r = FindObjectsByType<InteractableObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var VARIABLE in r) {
            obstaclePositions.AddRange(VARIABLE.GetOccupiedPositions());
        }
    }

    private void UpdateWalkable() {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                bool isWalkable = !obstaclePositions.Contains(position);  // Mark the obstacle positions as non-walkable
                grid[x, y].walkable = isWalkable;
            }
        }
    }

    // The A* pathfinding method
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        Node startNode = grid[start.x, start.y];
        Node targetNode = grid[end.x, end.y];

        openList.Clear();
        closedList.Clear();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // Get the node with the lowest fCost
            Node currentNode = GetNodeWithLowestFCost(openList);
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // If we reach the target, reconstruct the path
            if (currentNode.position == targetNode.position)
            {
                return RetracePath(startNode, currentNode);
            }

            // Evaluate each of the neighbors
            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedList.Contains(neighbor))
                    continue;

                int newGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newGCost < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newGCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return new List<Vector2Int>();  // Return an empty path if no path is found
    }

    // Get the node with the lowest fCost from the open list
    private Node GetNodeWithLowestFCost(List<Node> list)
    {
        Node lowestFCostNode = list[0];
        foreach (Node node in list)
        {
            if (node.fCost < lowestFCostNode.fCost)
                lowestFCostNode = node;
        }
        return lowestFCostNode;
    }

    // Get the neighbors of a node (up, down, left, right)
    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),  // Up
            new Vector2Int(1, 0),  // Right
            new Vector2Int(0, -1), // Down
            new Vector2Int(-1, 0), // Left
        };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPos = node.position + direction;
            if (IsValidPosition(neighborPos))
            {
                neighbors.Add(grid[neighborPos.x, neighborPos.y]);
            }
        }

        return neighbors;
    }

    // Check if a position is within the bounds of the grid
    private bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridSize.x && position.y >= 0 && position.y < gridSize.y;
    }

    // Get the Manhattan distance (heuristic) between two nodes
    private int GetDistance(Node a, Node b)
    {
        return Mathf.Abs(a.position.x - b.position.x) + Mathf.Abs(a.position.y - b.position.y);
    }

    // Retrace the path from the target to the start
    private List<Vector2Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
}
