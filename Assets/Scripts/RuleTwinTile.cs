using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class RuleTwinTile : RuleTile<RuleTwinTile.Neighbor> {
    [SerializeField]
    private RuleTile _twinRuleTile;

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Null: return tile == null;
            case Neighbor.NotNull: return tile != null;
        }

        if (tile is RuleOverrideTile ot)
            tile = ot.m_InstanceTile;

        switch (neighbor) {
            case TilingRuleOutput.Neighbor.This: return tile == this || tile == _twinRuleTile;
            case TilingRuleOutput.Neighbor.NotThis: return tile != this && tile != _twinRuleTile;
        }

        return true;
    }

    public class Neighbor : RuleTile.TilingRuleOutput.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
    }
}