using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class WallCustomTile : RuleTile<WallCustomTile.Neighbor> {
    [SerializeField]
    private RuleTile _cuttedTile, _bigWallsTile;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
        _cuttedTile.GetTileData(position, tilemap, ref tileData);
        if (Core.FogOfWarManager == null || _cuttedTile == null) {
            return;
        }

        if (!Core.FogOfWarManager.IsOpened(position + Vector3Int.up)) {
            _bigWallsTile.GetTileData(position, tilemap, ref tileData);
            return;
        }
    }

    public class Neighbor : RuleTile.TilingRuleOutput.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
    }
}