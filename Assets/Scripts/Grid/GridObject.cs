using System;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {
	
	public int X, Y, Layer;

	public bool Obstacle;

	public Vector3 GetObjectCenter()
	{
		var origin = transform.position - Vector3.one / 2; // нижний левый угол
		return origin + new Vector3(SizeX / 2f, SizeY / 2f, 0);
	}	
	public int3 Position => new int3(X, Y, Layer);
	[Min(0)]
	public Vector2Int MultiplyGridOffset;

	public int SizeX => MultiplyGridOffset.x + 1;
	public int SizeY => MultiplyGridOffset.y + 1;

	public void Init() {
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
	public List<Vector3> GetObjectCorners()
	{
		var corners = new List<Vector3>(4);
		corners.Add(transform.position - Vector3.one / 2); // нижний левый
		corners.Add(transform.position - Vector3.one / 2 + new Vector3(SizeX, 0)); // нижний правый
		corners.Add(transform.position - Vector3.one / 2 + new Vector3(SizeX, SizeY)); // верхний правый
		corners.Add(transform.position - Vector3.one / 2 + new Vector3(0, SizeY)); // верхний левый
		return corners;
	}

}