using System.Collections.Generic;
using UnityEngine;

public class MockPathfindService : IPathfindService{
	
	public List<Vector2> FindPath(Vector2 start, Vector2 end) {
		var path = new List<Vector2>();
		for (int x = (int)start.x; x <= end.x; x++) {
			for (int y = (int)start.y; y <= end.y; y++) {
				path.Add(new Vector2(x, y));
			}
		}
		return path;
	}
}