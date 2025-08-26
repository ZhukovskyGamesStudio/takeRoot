using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;

public class IterativeDeepeningAStar : IPathfindService {
    private const float FOUND = -1;

    private readonly SimpleGraph _graph;

    public IterativeDeepeningAStar(SimpleGraph graph) {
        _graph = graph;
    }

    public List<Vector2> FindPath(Vector2 start, Vector2 end) {
        int3 startInt = new((int)start.x, (int)start.y, 0); //TODO: change when layer added
        int3 goalInt = new((int)end.x, (int)end.y, 0); //TODO: change when layer added
        List<int3> path = FindPath(_graph, startInt, goalInt);

        List<Vector2> result = new();
        foreach (int3 pos in path) {
            result.Add(new Vector2(pos.x, pos.y));
        }

        return result;
    }

    public List<int3> FindPath(SimpleGraph graph, int3 startPos, int3 endPos) {
        if (!graph.TryGetNode(startPos, out SimpleNode start) || !graph.TryGetNode(endPos, out SimpleNode goal)) {
            return null;
        }

        Profiler.BeginSample("Iterative Deepening A*");
        float threshold = Heuristic(startPos, endPos);
        List<SimpleNode> path = new((int)threshold) { start };
        List<SimpleEdge> pathEdges = new();
        HashSet<SimpleNode> visited = new((int)threshold);

        while (true) {
            visited.Clear();
            float temp = Search(start, goal, 0, threshold, path, pathEdges, visited);

            if (temp == FOUND) {
                List<int3> result = new();
                foreach (SimpleNode node in path) {
                    result.Add(node.pos);
                }

                Profiler.EndSample();
                return result;
            }

            if (temp == float.PositiveInfinity) {
                Profiler.EndSample();
                return null;
            }

            threshold = temp;
        }
    }

    private float Search(SimpleNode current, SimpleNode goal, float g, float threshold, List<SimpleNode> path, List<SimpleEdge> pathEdges,
        HashSet<SimpleNode> visited) {
        float f = g + Heuristic(current.pos, goal.pos);

        if (f > threshold) {
            return f;
        }

        if (current == goal) {
            return FOUND;
        }

        visited.Add(current);
        float min = float.PositiveInfinity;

        foreach (SimpleEdge edge in current.outgoingEdges) {
            SimpleNode neighbor = edge.destinationNode;

            if (visited.Contains(neighbor)) {
                continue;
            }

            pathEdges.Add(edge);
            path.Add(neighbor);
            float t = Search(neighbor, goal, g + edge.weight, threshold, path, pathEdges, visited);

            if (t == FOUND) {
                return FOUND;
            }

            if (t < min) {
                min = t;
            }

            if (pathEdges.Count > 0) {
                pathEdges.RemoveAt(pathEdges.Count - 1);
            }

            path.RemoveAt(path.Count - 1);
        }

        return min;
    }

    private float Heuristic(int3 a, int3 b) {
        return math.abs(a.x - b.x) + math.abs(a.y - b.y);
        //return math.sqrt(math.pow(a.x - b.x, 2) + math.pow(a.y - b.y, 2));
    }
}