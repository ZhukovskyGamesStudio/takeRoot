using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class FloorPatternTile : TileBase {
    [SerializeField]
    private Sprite _evenXEvenY, _evenXOddY, _oddXEvenY, _oddXOddY;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
        base.GetTileData(position, tilemap, ref tileData);
        if (position.x % 2 == 0) {
            tileData.sprite = position.y % 2 == 0 ? _evenXEvenY : _evenXOddY;
        } else {
            tileData.sprite = position.y % 2 == 0 ? _oddXEvenY : _oddXOddY;
        }
    }
}