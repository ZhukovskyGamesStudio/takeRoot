using System.Collections.Generic;
using UnityEngine;

public interface IPathfindService : IService {
    public List<Vector2> FindPath(Vector2 start, Vector2 end);
}