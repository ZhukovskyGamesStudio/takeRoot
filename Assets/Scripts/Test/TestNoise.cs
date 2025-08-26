using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WorldObjects;

public class TestNoise : MonoBehaviour {
    private void Update() {
        if (SceneManager.GetActiveScene().name == "EnemyTestScene") {
            TestMakeNoiseOnMouseClick();
        }
    }

    private void TestMakeNoiseOnMouseClick() {
        if (Input.GetMouseButtonDown(0)) {
            int radius = 10;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int pos = new(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

            MakeNoise(pos, radius);
            Debug.Log($"Noise maked at {pos.x}:{pos.y} with {radius} radius");
        }
    }

    private void MakeNoise(Vector2Int noisePosition, int noiseRadius) {
        Listener[] listeners = FindObjectsByType<Listener>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        HashSet<Vector2Int> noisePoints = new();

        Vector2Int topLeft = new(noisePosition.x - noiseRadius, noisePosition.y + noiseRadius);
        Vector2Int botRight = new(noisePosition.x + noiseRadius, noisePosition.y - noiseRadius);

        for (int x = topLeft.x; x < botRight.x; x++)
        for (int y = topLeft.y; y > botRight.y; y--) {
            noisePoints.Add(new Vector2Int(x, y));
        }

        foreach (Listener listener in listeners) {
            foreach (Vector2Int occupiedPosition in listener.Gridable.GetOccupiedPositions()) {
                if (noisePoints.Contains(occupiedPosition)) {
                    listener.HasHeard?.Invoke(noisePosition);
                }
            }
        }
    }
}