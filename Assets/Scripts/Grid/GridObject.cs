using System.Collections.Generic;
using CodeBase.Services;
using UnityEngine;

public class GridObject : MonoBehaviour {
    private IGridService _grid;
    public int X, Y, Layer;

    public bool Obstacle;
    public int3 Position => new(X, Y, Layer);

    [Min(0)]
    public Vector2Int MultiplyGridOffset;

    public int SizeX => MultiplyGridOffset.x + 1;
    public int SizeY => MultiplyGridOffset.y + 1;

    private void Start() {
        _grid = ServiceLocator.Container.Single<IGridService>();
    }

    public void Init() {
        X = (int)transform.position.x;
        Y = (int)transform.position.y;
    }

    public Vector3 GetObjectCenter() {
        Vector3 origin = transform.position - Vector3.one / 2; // нижний левый угол
        return origin + new Vector3(SizeX / 2f, SizeY / 2f, 0);
    }

    public bool IsObstacle(int3 pos) {
        return Obstacle && pos.x >= X && pos.x < X + MultiplyGridOffset.x && pos.y >= Y && pos.y < Y + MultiplyGridOffset.y && pos.z == Layer;
    }

    public void UpdatePosition() { }

    public List<Vector3> GetObjectCorners() {
        List<Vector3> corners = new(4);
        corners.Add(transform.position - Vector3.one / 2); // нижний левый
        corners.Add(transform.position - Vector3.one / 2 + new Vector3(SizeX, 0)); // нижний правый
        corners.Add(transform.position - Vector3.one / 2 + new Vector3(SizeX, SizeY)); // верхний правый
        corners.Add(transform.position - Vector3.one / 2 + new Vector3(0, SizeY)); // верхний левый
        return corners;
    }

    public void Destroy() {
        for (int y = Y; y < Y + SizeY; y++)
        for (int x = X; x < X + SizeX; x++) {
            _grid.FreeTile(x, y);
        }
    }
}