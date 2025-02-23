using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using WorldObjects;


public class TestNoise : MonoBehaviour
{
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "EnemyTestScene")
            TestMakeNoiseOnMouseClick();
    }

    private void TestMakeNoiseOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var radius = 10;
            
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var pos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
            
            MakeNoise(pos, radius);
            Debug.Log($"Noise maked at {pos.x}:{pos.y} with {radius} radius");
        }
    }

    private void MakeNoise(Vector2Int noisePosition, int noiseRadius)
    {
        var listeners = FindObjectsByType<Listener>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        
        HashSet<Vector2Int> noisePoints = new HashSet<Vector2Int>();
            
        var topLeft = new Vector2Int(noisePosition.x - noiseRadius, noisePosition.y + noiseRadius);
        var botRight = new Vector2Int(noisePosition.x + noiseRadius, noisePosition.y - noiseRadius);
            
        for (int x = topLeft.x; x < botRight.x; x++)
        for (int y = topLeft.y; y > botRight.y; y--)
        {
            noisePoints.Add(new Vector2Int(x, y));
        }
        foreach (Listener listener in listeners)
        {
            foreach (Vector2Int occupiedPosition in listener.Gridable.GetOccupiedPositions())
                if (noisePoints.Contains(occupiedPosition))
                    listener.HasHeard?.Invoke(noisePosition);
        }
    }
}