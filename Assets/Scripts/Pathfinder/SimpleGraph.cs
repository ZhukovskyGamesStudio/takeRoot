using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleGraph
{
    private Dictionary<int3, SimpleNode> _nodesMap = new();
    public Dictionary<int3, SimpleNode> NodesMap => _nodesMap;
    
    public void SetNode(int3 pos)
    {
        if (_nodesMap.TryGetValue(pos, out var node))
        {
            return;
        }
        var newNode = new SimpleNode(pos)
        {
        };
        _nodesMap.Add(pos, newNode);
    }
    public void SetEdge(int3 start, int3 dest, float weight, bool isUndirected) 
    {
        if (_nodesMap.TryGetValue(start, out SimpleNode startNode))
            if (_nodesMap.TryGetValue(dest, out SimpleNode destNode))
            {
                var existingEdge = startNode.outgoingEdges.FirstOrDefault(e => e.from == startNode && e.destinationNode == destNode);
                if (existingEdge != null)
                {
                    existingEdge.weight = weight;
                }
                else
                {
                    var edge = new SimpleEdge(startNode ,destNode)
                    {
                        weight = weight,
                    };
                    startNode.outgoingEdges.Add(edge);
                }
            }
            else Debug.LogWarning($"{dest.x}:{dest.y}:{dest.z} does not exist as destination node");
        else Debug.LogWarning($"{start.x}:{start.y}:{start.z} does not exist as start node");


        if (isUndirected)
        {
            SetEdge(dest, start, weight, false);
        }
    }

    public bool TryGetNode(int3 pos, out SimpleNode node)
    {
        return _nodesMap.TryGetValue(pos, out node);
    }
    
}