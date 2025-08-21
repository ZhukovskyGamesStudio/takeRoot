using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class AStarPathfindingVertical : MonoBehaviour
{
    [SerializeField] private GameObject pathSquare;
    [SerializeField] int width = 1000, height = 1000, layers = 4;
    

    //private readonly List<Node> _neighbors = new List<Node>(5);
    private readonly List<Point> _path = new List<Point>(500);
    private readonly int[] _dX = { 0, 0, -1, 1 };
    private readonly int[] _dY = { -1, 1, 0, 0 };

    private NodeData[,,] _grid;
    private bool[,,] _walkableMap;
    private PriorityHeap<Point> _openQueue;
    private Point[] _neighbors = new Point[5];


    private WarpManager _warpManager;


    private void Awake()
    {
        ObsoleteCoreEntryPoint.AStarPathfindingVertical = this;
        _walkableMap = new bool[height, width, layers];
        _grid = new NodeData[width, height, layers];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                for (int k = 0; k < layers; k++)
                    _walkableMap[i, j, k] = true;
        _openQueue = new PriorityHeap<Point>();
    }

    public void Start()
    {
        _warpManager =
            FindObjectsByType<WarpManager>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)[0]; //TODO: singleton

        CreateMap();

        //var startNode = new Point(5, -6, 1);
        //var endNode = new Point(3, 3, 3);
        //var path = FindPath(startNode, new[] { endNode}, out _);
        //if (path == null)
        //{
        //    Debug.Log("No path found");
        //    return;
        //}
        //foreach (Point step in path)
        //{
        //    Debug.Log($"Path step: {step.X}:{step.Y} | {step.Layer}");
        //}
    }

    private void CreateMap()
    {
        var worldLayers = FindObjectsByType<WorldLayer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (WorldLayer worldLayer in worldLayers)
        {
            foreach (Gridable gridable in worldLayer.GetComponentsInChildren<Gridable>(false))
            foreach (Vector2Int pos in gridable.GetOccupiedLocalPositions())
            {
                var point = new Point(pos.x, pos.y, worldLayer.Layer);
                if (_grid[pos.x, pos.y, worldLayer.Layer].walkable)
                    SetWalkable(pos.x, pos.y, worldLayer.Layer, !gridable.IsBlockingPath);
                //Debug.Log($"Map point: {point.X}:{point.Y} | {point.Layer}");
                if (_walkableMap[pos.x, pos.y, worldLayer.Layer])
                    _walkableMap[pos.x, pos.y, worldLayer.Layer] = !gridable.IsBlockingPath;
            }
        }
    }

    public List<Point> FindPath(Point start, Point end, out bool isPathExist)
    {
        Profiler.BeginSample("FindPath");
        isPathExist = true;
        Array.Clear(_grid, 0, _grid.Length);
        _openQueue.Clear();

        Point goal = end;
        ref var startNode = ref _grid[start.X, start.Y, start.Layer];
        startNode.gCost = 0;
        startNode.hCost = Heuristic(start, goal);
        startNode.fCost = (short)(startNode.gCost + startNode.hCost);
        startNode.parent = start;
        _openQueue.Enqueue(start, startNode.fCost);

        int counter = 0;
        while (_openQueue.Count > 0)
        {
            counter++;
            var current = _openQueue.Dequeue();
            ref var currentNode = ref _grid[current.X, current.Y, current.Layer];

            if (current.Equals(goal))
            {
                return Retrace(start, current);
            }

            currentNode.closed = true;

            int neighborCount = GetNeighbors(current, _neighbors);
            for (int i = 0; i < neighborCount; i++)
            {
                var neighbor = _neighbors[i];
                ref var neighborNode = ref _grid[neighbor.X, neighbor.Y, neighbor.Layer];

                if (!_walkableMap[neighbor.X, neighbor.Y, neighbor.Layer] || neighborNode.closed)
                    continue;

                short tentativeG = (short)(currentNode.gCost + 1); // Manhattan: 1 per move
                
                if (tentativeG < neighborNode.gCost || neighborNode.gCost == 0)
                {
                    neighborNode.gCost = tentativeG;
                    neighborNode.hCost = Heuristic(neighbor, goal);
                    neighborNode.fCost = (short)(neighborNode.gCost + neighborNode.hCost);
                    neighborNode.parent = current;
                    _openQueue.Enqueue(neighbor, neighborNode.fCost);
                }
            }
        }

        isPathExist = false;
        Profiler.EndSample();
        return null;
    }
    public void SetWalkable(int x, int y, int layer, bool walkable)
    {
        _grid[x, y, layer].walkable = walkable;
    }
    private int GetNeighbors(Point p, Point[] output)
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            int nx = p.X + _dX[i], ny = p.Y + _dY[i], layer = p.Layer;
            if (nx >= 0 && ny >= 0 && nx < width && ny < height && _walkableMap[nx, ny, layer])
                output[count++] = new Point(nx, ny, layer);
        }

        if (_warpManager.warps.TryGetValue(p, out var warpTarget))
        {
            if (_walkableMap[warpTarget.X, warpTarget.Y, warpTarget.Layer])
                output[count++] = warpTarget;
        }

        return count;
    }

    private List<Point> Retrace(Point start, Point end)
    {
        var path = new List<Point>();
        Point current = end;
        
        while (!current.Equals(start))
        {
            path.Add(current);
            ref var node = ref _grid[current.X, current.Y, current.Layer];
            if (current.Equals(node.parent))
                break;
            //var sprite = Instantiate(pathSquare, new Vector3(current.X, current.Y), Quaternion.identity, this.transform).GetComponent<SpriteRenderer>();
            //sprite.color = Color.blue;
            current = node.parent;
        }

        path.Reverse();
        return path;
    }

    private short Heuristic(Point a, Point b)
    {
        return (short)(Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y));
    }
}
    public struct Point : IEquatable<Point>
    {
        public int X, Y, Layer;

        public Point(int x, int y, int layer)
        {
            this.X = x;
            this.Y = y;
            this.Layer = layer;
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y && Layer == other.Layer;
        }

        public override bool Equals(object obj)
        {
            return obj is Point other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Layer);
        }
    }

    public struct NodeData
    {
        public bool walkable;
        public short gCost;
        public short hCost;
        public short fCost;
        public bool closed;
        public Point parent;

        public NodeData(bool walkable = true)
        {
            this.walkable = true;
            gCost = 0;
            hCost = 0;
            fCost = 0;
            closed = false;
            parent = default;
        }
    }

    public class PriorityHeap<T>
    {
        private readonly List<(T item, int priority)> _elements = new();

        public int Count => _elements.Count;
        public void Clear() => _elements.Clear();

        public void Enqueue(T item, int priority)
        {
            _elements.Add((item, priority));
            HeapifyUp(_elements.Count - 1);
        }

        public T Dequeue()
        {
            var result = _elements[0].item;
            var last = _elements[^1];
            _elements[0] = last;
            _elements.RemoveAt(_elements.Count - 1);
            HeapifyDown(0);
            return result;
        }

        public bool Contains(T item)
        {
            return _elements.Exists(e => e.item.Equals(item));
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_elements[index].priority >= _elements[parent].priority)
                    break;

                (_elements[index], _elements[parent]) = (_elements[parent], _elements[index]);
                index = parent;
            }
        }

        private void HeapifyDown(int index)
        {
            int count = _elements.Count;
            while (true)
            {
                int smallest = index;
                int left = index * 2 + 1;
                int right = index * 2 + 2;

                if (left < count && _elements[left].priority < _elements[smallest].priority)
                    smallest = left;

                if (right < count && _elements[right].priority < _elements[smallest].priority)
                    smallest = right;

                if (smallest == index)
                    break;

                (_elements[index], _elements[smallest]) = (_elements[smallest], _elements[index]);
                index = smallest;
            }
        }
    }