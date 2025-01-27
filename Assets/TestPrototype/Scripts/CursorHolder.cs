using UnityEngine;

public class CursorHolder : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        PlayerCursor[] cursors = FindObjectsByType<PlayerCursor>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (PlayerCursor cursor in cursors) {
            cursor.transform.SetParent(transform);
        }
    }
}