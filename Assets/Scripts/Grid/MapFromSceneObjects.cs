using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class MapFromSceneObjects : MonoBehaviour{

	public Dictionary<int3, bool> Map = new Dictionary<int3, bool>(); // false - free, true - obstacle
	
	public List<GridObject> GridObjects; 

	public void CreateMap() {
		var gridObjects = Object.FindObjectsByType<GridObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

		foreach (GridObject obj in gridObjects) {
			for (int x = obj.X; x <= obj.X + obj.MultiplyGridOffset.x; x++) {
				for (int y = obj.Y; y <= obj.Y + obj.MultiplyGridOffset.y; y++) {
					var pos = new int3(x, y, obj.Layer);
					if (!Map.TryAdd(pos, obj.Obstacle)) {
						Map[pos] = Map[pos] == false ? obj.Obstacle : Map[pos];
					}
				}
			}
		}
	}

	public SimpleGraph CreateSimpleGraph() {
		CreateMap();
		var graph = new SimpleGraph();
		foreach (var kvp in Map) {
			if (!kvp.Value) {
				graph.SetNode(kvp.Key);
			}

			var pos = kvp.Key;
			for (int j = -1; j <= 1; j += 2) {

				var neighbour = new int3(pos.x + j, pos.y, pos.z);
				if (graph.NodesMap.TryGetValue(pos, out _) && graph.NodesMap.TryGetValue(neighbour, out _)) {
					graph.SetEdge(pos, neighbour, 1, true);
					
				}
				neighbour = new int3(pos.x, pos.y + j, pos.z);
				if (graph.NodesMap.TryGetValue(pos, out _) && graph.NodesMap.TryGetValue(neighbour, out _)) {
					graph.SetEdge(pos, neighbour, 1, true);
				}
			}
		}
		return graph;
	}
	
	
	
	
}