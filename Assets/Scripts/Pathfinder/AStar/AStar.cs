using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class AStar : IPathfindService
{
    private readonly SimpleGraph _graph;

    public AStar(SimpleGraph graph)
    {
        _graph = graph;
    }

    public List<Vector2> FindPath(Vector2 start, Vector2 end)
    {
        var startPos = new int3((int)start.x, (int)start.y, 0);
        var endPos = new int3((int)end.x, (int)end.y, 0);
        var path = FindPathInternal(_graph, startPos, endPos);
        
        if (path == null)
            return null;

        return path.Select(p => new Vector2(p.x, p.y)).ToList();
    }

    private List<int3> FindPathInternal(SimpleGraph graph, int3 startPos, int3 endPos)
    {
        if (!graph.NodesMap.TryGetValue(startPos, out var startNode) ||
            !graph.NodesMap.TryGetValue(endPos, out var endNode))
            return null;

        var openSet = new SortedSet<(float f, int insertOrder, SimpleNode node)>();
        var cameFrom = new Dictionary<SimpleNode, SimpleNode>();
        var gScore = new Dictionary<SimpleNode, float>();
        var fScore = new Dictionary<SimpleNode, float>();
        int insertCounter = 0;

        gScore[startNode] = 0;
        fScore[startNode] = Heuristic(startPos, endPos);
        openSet.Add((fScore[startNode], insertCounter++, startNode));

        while (openSet.Count > 0)
        {
            var current = openSet.Min.node;
            openSet.Remove(openSet.Min);

            if (current == endNode)
            {
                var result = new List<int3>();
                while (cameFrom.ContainsKey(current))
                {
                    result.Add(current.pos);
                    current = cameFrom[current];
                }
                result.Add(startPos);
                result.Reverse();
                return result;
            }

            foreach (var edge in current.outgoingEdges)
            {
                var neighbor = edge.destinationNode;
                float tentativeG = gScore[current] + edge.weight;

                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor.pos, endPos);
                    openSet.Add((fScore[neighbor], insertCounter++, neighbor));
                }
            }
        }

        return null;
    }

    private float Heuristic(int3 a, int3 b)
    {
        return math.abs(a.x - b.x) + math.abs(a.y - b.y);
    }
}
