using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    public readonly Dictionary<Point, Point> warps = new Dictionary<Point, Point>();

    private void Awake()
    {
        Core.WarpManager = this;
    }

    public void Init()
    {
        foreach (Warp warp in FindObjectsByType<Warp>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            warps.Add(warp.startPoint.Point, warp.destination.Point);
        }
    }

    public void AddWarp(Point warpPos, Point warpDestination)
    {
        warps.Add(warpPos, warpDestination);
    }
}