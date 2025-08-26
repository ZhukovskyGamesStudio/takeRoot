using System.Collections.Generic;
using UnityEngine;

public class MapFromSceneObjects : MonoBehaviour {
    public Dictionary<int3, bool> Map = new(); // false - free, true - obstacle

    public SimpleGraph Graph;

    public void CreateMap() {
        GridObject[] gridObjects = FindObjectsByType<GridObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (GridObject obj in gridObjects) {
            obj.Init();
            for (int x = obj.X; x <= obj.X + obj.MultiplyGridOffset.x; x++) {
                for (int y = obj.Y; y <= obj.Y + obj.MultiplyGridOffset.y; y++) {
                    int3 pos = new(x, y, obj.Layer);
                    if (!Map.TryAdd(pos, obj.Obstacle)) {
                        Map[pos] = Map[pos] == false ? obj.Obstacle : Map[pos];
                    }
                }
            }
        }
    }

    public SimpleGraph CreateSimpleGraph() {
        SimpleGraph graph = new();
        foreach (KeyValuePair<int3, bool> kvp in Map) {
            if (!kvp.Value) {
                graph.SetNode(kvp.Key);
            }

            int3 pos = kvp.Key;
            for (int j = -1; j <= 1; j += 2) {
                int3 neighbour = new(pos.x + j, pos.y, pos.z);
                if (graph.NodesMap.TryGetValue(pos, out _) && graph.NodesMap.TryGetValue(neighbour, out _)) {
                    graph.SetEdge(pos, neighbour, 1, true);
                }

                neighbour = new int3(pos.x, pos.y + j, pos.z);
                if (graph.NodesMap.TryGetValue(pos, out _) && graph.NodesMap.TryGetValue(neighbour, out _)) {
                    graph.SetEdge(pos, neighbour, 1, true);
                }
            }
        }

        Graph = graph;
        return graph;
    }

    private void OnDrawGizmos() {
        if (Graph == null) {
            return;
        }

        foreach (KeyValuePair<int3, SimpleNode> kvp in Graph.NodesMap) {
            int3 pos = kvp.Key;
            SimpleNode node = kvp.Value;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(new Vector3(pos.x, pos.y), 0.6f);
            foreach (SimpleEdge edge in node.outgoingEdges) {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector3(edge.from.pos.x, edge.from.pos.y),
                    new Vector3(edge.destinationNode.pos.x, edge.destinationNode.pos.y));
            }
        }
    }
}