using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "FogOfWarConfig", menuName = "Scriptable Objects/FogOfWarConfig", order = 0)]
public class FogOfWarConfig : ScriptableObject {
    [field: SerializeField]
    public TileBase BlackTile { get; set; }
    [field: SerializeField]
    public TileBase GreyTile { get; set; }

    [field: SerializeField]
    [Min(1)]
    public int ViewRadius { get; private set; }
    
    [field: SerializeField]
    [Min(0)]
    public int UpdateFogCooldown { get; private set; }
}