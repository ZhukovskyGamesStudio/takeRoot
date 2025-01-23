using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

[CustomEditor(typeof(ResourcesTable))]
public class ResourcesTableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        ResourcesTable resourcesTable = (ResourcesTable)target;

        if (GUILayout.Button("Find and Populate Resources"))
        {
            PopulateResourceLists(resourcesTable);
        }
    }

    private void PopulateResourceLists(ResourcesTable resourcesTable) {
        // Clear the existing lists
        resourcesTable.ResourceViewPrefabs.Clear();
        resourcesTable.ResourceUiViewPrefabs.Clear();

        resourcesTable.ResourceViewPrefabs.AddRange(FindAssetsOfType<ResourceView>());
        resourcesTable.ResourceUiViewPrefabs.AddRange(FindAssetsOfType<ResouseUiView>());

        // Save the changes
        EditorUtility.SetDirty(resourcesTable);
        Debug.Log("Resources populated successfully.");
    }

    public static List<T> FindAssetsOfType<T>() where T : Object {
        string[] guids = AssetDatabase.FindAssets("t:Object");

        List<T> res = guids.Select(variable => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(variable)))
            .Where(resourceView => resourceView != null).ToList();

        return res;
    }
}