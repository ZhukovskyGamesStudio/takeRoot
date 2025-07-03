using System;
using UnityEngine;

public class GridObject : MonoBehaviour {
	
	public int X, Y, Layer;

	public bool Obstacle;
	
	public int3 Position => new int3(X, Y, Layer);
	[Min(0)]
	public Vector2Int MultiplyGridOffset;

	public void Awake() {
		X = (int)transform.position.x;
		Y = (int)transform.position.y;
	}

	public bool IsObstacle(int3 pos) {
		return Obstacle && (
		       pos.x >= X && pos.x < X + MultiplyGridOffset.x &&
		       pos.y >= Y && pos.y < Y + MultiplyGridOffset.y &&
		       pos.z == Layer);
	}
	
	public void UpdatePosition() {
	}
}