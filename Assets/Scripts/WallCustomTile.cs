using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class WallCustomTile : RuleTwinTile {
    [SerializeField]
    private RuleTile _cuttedTile, _bigWallsTile, _wallOverStreetTile;

    [SerializeField]
    private TileBase _streetTile;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
        _bigWallsTile.GetTileData(position, tilemap, ref tileData);

        /*
        if (ObsoleteCoreEntryPoint.FogOfWarManager == null || _cuttedTile == null) {
            return;
        }

        if (!ObsoleteCoreEntryPoint.FogOfWarManager.IsOpened(position + Vector3Int.up)) {
            _bigWallsTile.GetTileData(position, tilemap, ref tileData);
            return;
        }*/

        // Проверка тайла под текущим
        /*Vector3Int belowPos = position + Vector3Int.down;
        TileBase belowTile = tilemap.GetTile(belowPos);
        if (belowTile == _streetTile) {
            _wallOverStreetTile.GetTileData(position, tilemap, ref tileData);
            return;
        }*/
    }
}