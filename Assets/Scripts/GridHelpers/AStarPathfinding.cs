using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AStarPathfinding : MonoBehaviour {
    public static AStarPathfinding Instance;
    private readonly HashSet<Node> _closedList = new HashSet<Node>();

    private readonly HashSet<Vector2Int> _obstaclePositions = new HashSet<Vector2Int>(); // List of obstacles in the form of grid positions
    private readonly HashSet<Node> _openList = new HashSet<Node>();

    private Dictionary<Vector2Int, Node> _grid;
    private HashSet<Vector2Int> _wallsPositions = new HashSet<Vector2Int>();

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        StartCoroutine(WaitAndInit());
    }

    public bool IsInited() {
        return _grid != null;
    }

    private IEnumerator WaitAndInit() {
        yield return new WaitForEndOfFrame();
        InitializeGrid();
        InvokeRepeating(nameof(UpdateWalkable), 0, 1);
    }

    // Initialize the grid with walkable and blocked nodes
    private void InitializeGrid() {
        Rect rect = Core.GridManager.GridSize;
        Vector2Int min = new Vector2Int((int)rect.x, (int)rect.y);
        Vector2Int max = new Vector2Int((int)rect.width, (int)rect.height);
        _grid = new Dictionary<Vector2Int, Node>((max.x - min.x) * (max.y - min.y));

        for (int x = min.x; x < max.x; x++) {
            for (int y = min.x; y < max.y; y++) {
                Vector2Int position = new Vector2Int(x, y);
                _grid.Add(position, new Node(position, true));
            }
        }
    }

    private void FindObstacles() {
        _obstaclePositions.Clear();
        _wallsPositions.Clear();
        Gridable[] r = FindObjectsByType<Gridable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Where(r => r.IsBlockingPath)
            .ToArray();
        foreach (Gridable gridable in r) {
            foreach (Vector2Int blockedPos in gridable.GetOccupiedPositions()) {
                _obstaclePositions.Add(blockedPos);
            }
        }
    }

    private void FindWalls() {
        Gridable[] walls = FindObjectsByType<Gridable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .Where(w => w.GetComponent<WallTile>() != null).ToArray();
        foreach (Gridable gridable in walls)
        foreach (Vector2Int wallPos in gridable.GetOccupiedPositions()) {
            _wallsPositions.Add(wallPos);
        }
    }

    private void UpdateWalkable() {
        FindObstacles();
        FindWalls();

        foreach (var cell in _grid.Values) {
            cell.Walkable = true; // Assume all walkable first
        }

        foreach (Vector2Int obstacle in _obstaclePositions) {
            if (_grid.TryGetValue(obstacle, out var cell)) {
                cell.Walkable = false;
            }
        }
    }

    // The A* pathfinding method
    public List<Vector2Int> FindPath(Vector2Int start, IEnumerable<Vector2Int> endCells, out bool isPathExist) {
        isPathExist = true;
        Node startNode = _grid[start];

        // Convert endCells to a HashSet for faster lookups
        HashSet<Node> targetNodes = new HashSet<Node>(endCells.Select(cell => _grid[cell]));

        _openList.Clear();
        _closedList.Clear();

        _openList.Add(startNode);

        int counter = 0;
        while (_openList.Count > 0 && counter < 500) {
            counter++;
            // Get the node with the lowest fCost
            Node currentNode = GetNodeWithLowestFCost(_openList);
            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            // Check if the current node is one of the target nodes
            if (targetNodes.Contains(currentNode)) {
                return RetracePath(startNode, currentNode); // Found the path to the nearest end cell
            }

            // Evaluate each of the neighbors
            foreach (Node neighbor in GetNeighbors(currentNode)) {
                if (!neighbor.Walkable || _closedList.Contains(neighbor))
                    continue;

                short newGCost = (short)(currentNode.GCost + GetDistance(currentNode, neighbor));
                if (newGCost >= neighbor.GCost && _openList.Contains(neighbor)) {
                    continue;
                }

                neighbor.GCost = newGCost;
                neighbor.HCost = GetDistance(neighbor, targetNodes); // Modified to consider multiple targets
                neighbor.Parent = currentNode;

                if (!_openList.Contains(neighbor)) {
                    _openList.Add(neighbor);
                }
            }
        }

        isPathExist = false;
        return new List<Vector2Int>(); // Return an empty list if no path is found
    }

    public List<Vector2Int> FindPathForZombies(Vector2Int start, IEnumerable<Vector2Int> endCells, int offsetX) {
        Node startNode = _grid[start];

        // Convert endCells to a HashSet for faster lookups
        HashSet<Node> targetNodes = new HashSet<Node>(endCells.Select(cell => _grid[cell]));

        _openList.Clear();
        _closedList.Clear();

        _openList.Add(startNode);

        int counter = 0;
        while (_openList.Count > 0 && counter < 500) {
            counter++;
            // Get the node with the lowest fCost
            Node currentNode = GetNodeWithLowestFCost(_openList);
            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            // Check if the current node is one of the target nodes
            if (targetNodes.Contains(currentNode)) {
                return RetracePath(startNode, currentNode); // Found the path to the nearest end cell
            }

            // Evaluate each of the neighbors
            foreach (Node neighbor in GetNeighbors(currentNode)) {
                if (!neighbor.Walkable || _closedList.Contains(neighbor))
                    continue;

                if (!IsWalkable(new Vector2Int(neighbor.PosX + offsetX, neighbor.PosY)))
                    continue;

                short newGCost = (short)(currentNode.GCost + GetDistance(currentNode, neighbor));
                if (newGCost >= neighbor.GCost && _openList.Contains(neighbor)) {
                    continue;
                }

                neighbor.GCost = newGCost;
                neighbor.HCost = GetDistance(neighbor, targetNodes); // Modified to consider multiple targets
                neighbor.Parent = currentNode;

                if (!_openList.Contains(neighbor)) {
                    _openList.Add(neighbor);
                }
            }
        }

        return new List<Vector2Int>(); // Return an empty list if no path is found
    }

    public List<Vector2Int> FindPathForZombiesWithWallsAsObstacleOld(Vector2Int start, IEnumerable<Vector2Int> endCells, int offsetX,
        IEnumerable<Vector2Int> occupiedCells) {
        HashSet<Node> occupiedStartNodes = new HashSet<Node>(occupiedCells.Select(cell => _grid[cell]));
        // Convert endCells to a HashSet for faster lookups
        HashSet<Node> targetNodes = new HashSet<Node>(endCells.Select(cell => _grid[cell]));

        Node closestNode = new Node(new Vector2Int(999, 999), true);
        foreach (Node occupiedStartNode in occupiedStartNodes) {
            if (GetDistance(occupiedStartNode, targetNodes) < GetDistance(closestNode, targetNodes))
                closestNode = occupiedStartNode;
        }

        Node startNode = closestNode;

        _openList.Clear();
        _closedList.Clear();

        _openList.Add(startNode);

        int counter = 0;
        while (_openList.Count > 0 && counter < 500) {
            counter++;
            // Get the node with the lowest fCost
            Node currentNode = GetNodeWithLowestFCost(_openList);
            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            // Check if the current node is one of the target nodes
            if (targetNodes.Contains(currentNode)) {
                return RetracePath(startNode, currentNode); // Found the path to the nearest end cell
            }

            // Evaluate each of the neighbors
            foreach (Node neighbor in GetNeighbors(currentNode)) {
                var neigbourPos = new Vector2Int(neighbor.PosX, neighbor.PosY);
                if (occupiedStartNodes.Contains(neighbor))
                    continue;
                if (_wallsPositions.Contains(neigbourPos) || _closedList.Contains(neighbor))
                    continue;
                if (_wallsPositions.Contains(new Vector2Int(neighbor.PosX + offsetX, neighbor.PosY)))
                    continue;

                short newGCost = (short)(currentNode.GCost + GetDistance(currentNode, neighbor));
                if (newGCost >= neighbor.GCost && _openList.Contains(neighbor)) {
                    continue;
                }

                neighbor.GCost = newGCost;
                neighbor.HCost = GetDistance(neighbor, targetNodes); // Modified to consider multiple targets
                neighbor.Parent = currentNode;

                if (!_openList.Contains(neighbor)) {
                    _openList.Add(neighbor);
                }
            }
        }

        return new List<Vector2Int>();
    }

    public List<Vector2Int> FindPathForZombiesWithWallsAsObstacle(Vector2Int start, IEnumerable<Vector2Int> endCells, int offsetX) {
        Node startNode = _grid[start];

        // Convert endCells to a HashSet for faster lookups
        HashSet<Node> targetNodes = new HashSet<Node>(endCells.Select(cell => _grid[cell]));

        _openList.Clear();
        _closedList.Clear();

        _openList.Add(startNode);

        int counter = 0;
        while (_openList.Count > 0 && counter < 500) {
            counter++;
            // Get the node with the lowest fCost
            Node currentNode = GetNodeWithLowestFCost(_openList);
            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            // Check if the current node is one of the target nodes
            if (targetNodes.Contains(currentNode)) {
                return RetracePath(startNode, currentNode); // Found the path to the nearest end cell
            }

            // Evaluate each of the neighbors
            foreach (Node neighbor in GetNeighbors(currentNode)) {
                if (_closedList.Contains(neighbor))
                    continue;
                if (_wallsPositions.Contains(new Vector2Int(neighbor.PosX, neighbor.PosY)))
                    continue;
                if (_wallsPositions.Contains(new Vector2Int(neighbor.PosX + offsetX, neighbor.PosY)))
                    continue;

                short newGCost = (short)(currentNode.GCost + GetDistance(currentNode, neighbor));
                if (newGCost >= neighbor.GCost && _openList.Contains(neighbor)) {
                    continue;
                }

                neighbor.GCost = newGCost;
                neighbor.HCost = GetDistance(neighbor, targetNodes); // Modified to consider multiple targets
                neighbor.Parent = currentNode;

                if (!_openList.Contains(neighbor)) {
                    _openList.Add(neighbor);
                }
            }
        }

        return new List<Vector2Int>(); // Return an empty list if no path is found
    }

    // Get the node with the lowest fCost from the open list
    private Node GetNodeWithLowestFCost(HashSet<Node> list) {
        Node lowestFCostNode = list.First();
        foreach (Node node in list.Where(node => node.FCost < lowestFCostNode.FCost)) {
            lowestFCostNode = node;
        }

        return lowestFCostNode;
    }

    // Get the neighbors of a node (up, down, left, right)
    private HashSet<Node> GetNeighbors(Node node) {
        HashSet<Node> neighbors = new HashSet<Node>();

        Vector2Int[] directions = new Vector2Int[] {
            new Vector2Int(0, 1), // Up
            new Vector2Int(1, 0), // Right
            new Vector2Int(0, -1), // Down
            new Vector2Int(-1, 0), // Left
        };

        foreach (Vector2Int direction in directions) {
            Vector2Int neighborPos = new Vector2Int(node.PosX, node.PosY) + direction;
            if (IsValidPosition(neighborPos)) {
                neighbors.Add(_grid[neighborPos]);
            }
        }

        return neighbors;
    }

    // Check if a position is within the bounds of the grid
    private bool IsValidPosition(Vector2Int position) {
        return _grid.ContainsKey(position);
    }

    public static bool IsWalkable(Vector2Int position) {
        return Instance._grid[position].Walkable;
    }

    // Get the Manhattan distance (heuristic) between two nodes
    private short GetDistance(Node a, HashSet<Node> targets) {
        return targets.Min(target => GetDistance(a, target));
    }

    private short GetDistance(Node a, Node b) {
        return (short)(Mathf.Abs(a.PosX - b.PosX) + Mathf.Abs(a.PosY - b.PosY));
    }

    // Retrace the path from the target to the start
    private List<Vector2Int> RetracePath(Node startNode, Node endNode) {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(new Vector2Int(currentNode.PosX, currentNode.PosY));
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    public static Vector2Int FindClosestCellFromList(Vector2Int target, IEnumerable<Vector2Int> list) {
        float closestDistance = float.PositiveInfinity;
        Vector2Int closestCell = Vector2Int.zero;
        foreach (Vector2Int cell in list) {
            float distance = (cell - target).sqrMagnitude;
            if (distance >= closestDistance)
                continue;

            closestDistance = distance;
            closestCell = cell;
        }

        return closestCell;
    }
}