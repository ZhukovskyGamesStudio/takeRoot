using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gridable))]
public class GridableEditor : Editor {
    public override void OnInspectorGUI() {
        // Draw the default inspector
        DrawDefaultInspector();
        if (GUILayout.Button("Update grid positions")) {
            Gridable gridable = (Gridable)target;
            gridable.PositionChanged();
        }
    }
}