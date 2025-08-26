using UnityEngine;

public class GridService : IGridService {
    private readonly MapFromSceneObjects _map; // false - free, true - obstacle

    public GridService(MapFromSceneObjects map) {
        _map = map;
    }

    public bool IsOccupiedPos(int x, int y) {
        return _map.Map[new int3(x, y, 0)];
    }

    public bool IsOccupiedPos(Vector3 pos) {
        return _map.Map[new int3((int)pos.x, (int)pos.y, (int)pos.z)];
    }

    public void FreeTile(int x, int y) {
        _map.Map[new int3(x, y, 0)] = false;
        _map.CreateSimpleGraph();
    }

    public void FreeTile(Vector3 pos) {
        _map.Map[new int3((int)pos.x, (int)pos.y, (int)pos.z)] = false;
        _map.CreateSimpleGraph();
    }
}