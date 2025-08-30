#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using AI.Node;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(AI.Settler))]
public class SettlerEditor : Editor {
    private bool _showBehaviorTree = true;
    private Dictionary<BTNode, BTNodeDebugInfo> _nodeDebugInfos = new();
    private GUIStyle _richTextStyle;
    private BTNode _lastEvaluatedNode;

    private void OnEnable() {
        _richTextStyle = new GUIStyle(EditorStyles.label) {
            richText = true,
            normal = { textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black }
        };
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        DrawDefaultInspector();

        AI.Settler settler = (AI.Settler)target;
        if (settler == null) {
            return;
        }

        // Получаем корневой узел через рефлексию
        FieldInfo rootField = typeof(AI.Settler).GetField("_root", BindingFlags.NonPublic | BindingFlags.Instance);
        BTNode rootNode = rootField?.GetValue(settler) as BTNode;

        // Получаем последнюю оцененную ноду через рефлексию
        FieldInfo lastNodeField = typeof(BTNode).GetField("_state", BindingFlags.NonPublic | BindingFlags.Instance);
        _lastEvaluatedNode = null;
        FindLastEvaluatedNode(rootNode, lastNodeField);

        _showBehaviorTree = EditorGUILayout.Foldout(_showBehaviorTree, "Behavior Tree Debug", true);
        if (_showBehaviorTree && rootNode != null) {
            EditorGUI.indentLevel++;
            DrawBTNode(rootNode, 0);
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void FindLastEvaluatedNode(BTNode node, FieldInfo stateField) {
        if (node == null) {
            return;
        }

        // Проверяем, была ли нода оценена в текущем кадре
        BTNodeState state = (BTNodeState)stateField.GetValue(node);
        if (state != BTNodeState.Running && state != 0) // 0 - это default значение enum
        {
            _lastEvaluatedNode = node;
        }

        // Рекурсивно проверяем дочерние ноды
        if (node is Selector selector) {
            FieldInfo childrenField = typeof(Selector).GetField("_children", BindingFlags.NonPublic | BindingFlags.Instance);
            List<BTNode> children = (List<BTNode>)childrenField?.GetValue(selector);
            if (children != null) {
                foreach (BTNode child in children) {
                    FindLastEvaluatedNode(child, stateField);
                }
            }
        } else if (node is Sequence sequence) {
            FieldInfo childrenField = typeof(Sequence).GetField("_children", BindingFlags.NonPublic | BindingFlags.Instance);
            List<BTNode> children = (List<BTNode>)childrenField?.GetValue(sequence);
            if (children != null) {
                foreach (BTNode child in children) {
                    FindLastEvaluatedNode(child, stateField);
                }
            }
        }
    }

    private void DrawBTNode(BTNode node, int depth) {
        if (node == null || _richTextStyle == null) {
            return;
        }

        if (!_nodeDebugInfos.TryGetValue(node, out BTNodeDebugInfo debugInfo)) {
            debugInfo = new BTNodeDebugInfo {
                Name = node.Name,
                Depth = depth
            };
            _nodeDebugInfos[node] = debugInfo;
        }

        // Получаем текущее состояние через рефлексию
        FieldInfo stateField = typeof(BTNode).GetField("_state", BindingFlags.NonPublic | BindingFlags.Instance);
        BTNodeState currentState = (BTNodeState)stateField.GetValue(node);

        // Определяем, является ли нода активной в текущем кадре
        bool isActive = node == _lastEvaluatedNode ||
                        ((node is Selector || node is Sequence) && IsParentOfLastEvaluated(node, _lastEvaluatedNode));

        string indent = new(' ', depth * 15);
        string displayText = $"{indent}{GetColoredStateIcon(currentState, isActive)} {node.GetType().Name}: {node.Name}";

        if (node is Selector || node is Sequence) {
            debugInfo.IsExpanded = EditorGUILayout.Foldout(debugInfo.IsExpanded, displayText, true, _richTextStyle);

            if (debugInfo.IsExpanded) {
                EditorGUI.indentLevel++;
                List<BTNode> children = GetChildren(node);
                foreach (BTNode child in children) {
                    DrawBTNode(child, depth + 1);
                }

                EditorGUI.indentLevel--;
            }
        } else {
            EditorGUILayout.LabelField(displayText, _richTextStyle);
        }
    }

    private bool IsParentOfLastEvaluated(BTNode potentialParent, BTNode node) {
        if (node == null) {
            return false;
        }

        List<BTNode> children = GetChildren(potentialParent);
        foreach (BTNode child in children) {
            if (child == node || IsParentOfLastEvaluated(child, node)) {
                return true;
            }
        }

        return false;
    }

    private List<BTNode> GetChildren(BTNode node) {
        List<BTNode> children = new();

        if (node is Selector selector) {
            FieldInfo field = typeof(Selector).GetField("_children", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null) {
                children = (List<BTNode>)field.GetValue(selector) ?? new List<BTNode>();
            }
        } else if (node is Sequence sequence) {
            FieldInfo field = typeof(Sequence).GetField("_children", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null) {
                children = (List<BTNode>)field.GetValue(sequence) ?? new List<BTNode>();
            }
        } else if (node is ParallelSelector parallelSelector) {
            FieldInfo field = typeof(Selector).GetField("_children", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null) {
                children = (List<BTNode>)field.GetValue(parallelSelector) ?? new List<BTNode>();
            }
        }

        return children;
    }

    private string GetColoredStateIcon(BTNodeState state, bool isActive) {
        if (!isActive) {
            return "<color=#777777>■</color>"; // Серый для неактивных нод
        }

        switch (state) {
            case BTNodeState.Running:
                return "<color=#FFFF00>■</color>"; // Желтый
            case BTNodeState.Success:
                return "<color=#00FF00>■</color>"; // Зеленый
            case BTNodeState.Failure:
                return "<color=#FF0000>■</color>"; // Красный
            default:
                return "<color=#AAAAAA>■</color>"; // Серый по умолчанию
        }
    }
}

public class BTNodeDebugInfo {
    public string Name { get; set; }
    public int Depth { get; set; }
    public bool IsExpanded { get; set; } = true;
}
#endif