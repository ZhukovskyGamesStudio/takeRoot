using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializedDictionary<,>), true)]
public class SerializedDictionaryDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}