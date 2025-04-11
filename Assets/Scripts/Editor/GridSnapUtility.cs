using UnityEngine;
using UnityEditor;

public static class GridSnapUtility
{
    [MenuItem("Tools/Grid/Snap All Gridables to Grid %#g")]
    public static void SnapAllGridables()
    {
        Gridable[] gridables = Object.FindObjectsByType<Gridable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int count = 0;

        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Snap Gridables to Grid");
        int undoGroup = Undo.GetCurrentGroup();

        foreach (var g in gridables)
        {
            Transform t = g.transform;
            Vector3 original = t.position;
            Vector3 snapped = new Vector3(
                Mathf.Round(original.x),
                Mathf.Round(original.y),
                original.z
            );

            if (snapped != original)
            {
                Undo.RecordObject(t, "Snap Gridable");
                t.position = snapped;
                count++;
            }
        }

        Undo.CollapseUndoOperations(undoGroup);
        Debug.Log($"Snapped {count} Gridable object(s) to grid.");
    }
}