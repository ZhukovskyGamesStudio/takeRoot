using UnityEngine;

public interface IGridService : IService {
    bool IsOccupiedPos(int x, int y);
    bool IsOccupiedPos(Vector3 pos);
    void FreeTile(int x, int y);
    void FreeTile(Vector3 pos);
}