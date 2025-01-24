using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(SpriteSwapToggle), true)]
[CanEditMultipleObjects]
public class SpriteSwapToggleEditor : ToggleEditor {
    // SerializedProperty for the NeedPlaySound field
    private SerializedProperty _targetImage, _onSprite, _offSprite;

    protected override void OnEnable() {
        base.OnEnable();

        _targetImage = serializedObject.FindProperty("_targetImage");
        _onSprite = serializedObject.FindProperty("_onSprite");
        _offSprite = serializedObject.FindProperty("_offSprite");
    }

    // Override the Inspector GUI
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(_targetImage);
        EditorGUILayout.PropertyField(_onSprite);
        EditorGUILayout.PropertyField(_offSprite);

        serializedObject.ApplyModifiedProperties();
    }
}