using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class RuleTwinTile : RuleTile<RuleTwinTile.Neighbor> {
    [SerializeField]
    private List<TileBase> _twinRuleTiles;

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Null: return tile == null;
            case Neighbor.NotNull: return tile != null;
        }

        if (tile is RuleOverrideTile ot) {
            tile = ot.m_InstanceTile;
        }

        switch (neighbor) {
            case TilingRuleOutput.Neighbor.This: return tile == this || _twinRuleTiles.Contains(tile);
            case TilingRuleOutput.Neighbor.NotThis: return tile != this && !_twinRuleTiles.Contains(tile);
        }

        return true;
    }

    public class Neighbor : RuleTile.TilingRuleOutput.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
    }
}