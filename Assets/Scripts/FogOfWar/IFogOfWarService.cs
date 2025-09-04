
    using UnityEngine;

    public interface IFogOfWarService : IService {
        public bool IsOpened(Vector2Int cell);

        public bool IsOpened(Vector3Int cell);
    }
