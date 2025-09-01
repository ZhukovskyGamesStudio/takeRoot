using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class AStar : IPathfindService {
    private readonly MapFromSceneObjects _graph;
    private Dictionary<IPathfinderUser, Dictionary<Vector2, int>> _pathfinderChecks = new Dictionary<IPathfinderUser, Dictionary<Vector2, int>>();
    private List<PathData> _savedPaths = new(); //TODO: cleanup old paths;
    private int _skipChecks = 50;

    public AStar(MapFromSceneObjects graph) {
        _graph = graph;
    }

    public List<Vector2> FindPath(Vector2 start, Vector2 end, IPathfinderUser user) {
        if (_pathfinderChecks.TryGetValue(user, out var checks)) {
            if (checks.TryGetValue(end, out var result)) {
                if (result >= _skipChecks) {
                    _pathfinderChecks[user].Remove(end);
                }else {
                    checks[end]++;
                    return null;
                }
            }
        } else {
            _pathfinderChecks[user] = new Dictionary<Vector2, int>();
        }

        int3 startPos = new((int)start.x, (int)start.y, 0);
        int3 endPos = new((int)end.x, (int)end.y, 0);
        var savedPath = _savedPaths.FirstOrDefault(s => (s.Start.Equals(startPos) || s.Path.Contains(start)) && s.End.Equals(endPos));
        if (savedPath != null) {
            return savedPath.Path;
        }
        List<int3> path = FindPathInternal(_graph.Graph, startPos, endPos);

        if (path == null) {
            _pathfinderChecks[user].Add(end, 0);
            return null;
        }

        var pathList = path.Select(p => new Vector2(p.x, p.y)).ToList();
        if (_savedPaths.Count > 200) {
            _savedPaths.RemoveRange(0, 50);
        }
        _savedPaths.Add(new PathData(startPos, endPos, pathList));
        return pathList;
    }

    private List<int3> FindPathInternal(SimpleGraph graph, int3 startPos, int3 endPos) {
        if (!graph.NodesMap.TryGetValue(startPos, out SimpleNode startNode) || !graph.NodesMap.TryGetValue(endPos, out SimpleNode endNode)) {
            return null;
        }

        SortedSet<(float f, int insertOrder, SimpleNode node)> openSet = new(new NodeComparer());
        Dictionary<SimpleNode, SimpleNode> cameFrom = new();
        Dictionary<SimpleNode, float> gScore = new();
        Dictionary<SimpleNode, float> fScore = new();
        int insertCounter = 0;

        gScore[startNode] = 0;
        fScore[startNode] = Heuristic(startPos, endPos);
        openSet.Add((fScore[startNode], insertCounter++, startNode));

        while (openSet.Count > 0) {
            SimpleNode current = openSet.Min.node;
            openSet.Remove(openSet.Min);

            if (current == endNode) {
                List<int3> result = new();
                while (cameFrom.ContainsKey(current)) {
                    result.Add(current.pos);
                    current = cameFrom[current];
                }

                result.Add(startPos);
                result.Reverse();
                return result;
            }

            foreach (SimpleEdge edge in current.outgoingEdges) {
                SimpleNode neighbor = edge.destinationNode;
                float tentativeG = gScore[current] + edge.weight;

                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor]) {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor.pos, endPos);
                    openSet.Add((fScore[neighbor], insertCounter++, neighbor));
                }
            }
        }

        return null;
    }

    private float Heuristic(int3 a, int3 b) {
        return math.abs(a.x - b.x) + math.abs(a.y - b.y);
    }

    private class NodeComparer : IComparer<(float f, int insertOrder, SimpleNode node)> {
        public int Compare((float f, int insertOrder, SimpleNode node) x, (float f, int insertOrder, SimpleNode node) y) {
            int cmp = x.f.CompareTo(y.f);
            if (cmp != 0) {
                return cmp;
            }

            cmp = x.insertOrder.CompareTo(y.insertOrder);
            if (cmp != 0) {
                return cmp;
            }

            return x.node.GetHashCode().CompareTo(y.node.GetHashCode()); // или ReferenceEquals
        }
    }

    private class PathData {
        public int3 Start;
        public int3 End;
        public List<Vector2> Path;

        public PathData(int3 start, int3 end, List<Vector2> path) {
            Start = start;
            End = end;
            Path = path;
        }
    }
}