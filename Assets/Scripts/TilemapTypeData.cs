using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTypeData : MonoBehaviour {
    [field: SerializeField]
    public TilemapType Type { get; set; }

    [field: SerializeField]
    public bool IsMain { get; set; }

    [field: SerializeField]
    public Tilemap Tilemap { get; set; }

    private void Awake() {
        if (Tilemap == null)
            Tilemap = GetComponent<Tilemap>();
    }
}

[Serializable]
public enum TilemapType {
    Grass,
    Road,
    Sidewalk,
    Floor,
    Water,
    Other,
    FogOfWarBlack,
    FogOfWarGrey
}