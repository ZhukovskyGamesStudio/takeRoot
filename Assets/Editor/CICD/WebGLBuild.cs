using System;
using UnityEditor;
using UnityEditor.Build.Profile;
using UnityEngine;

public static class WebGLBuild {
    private const string WebglGithubPages = "WebGLGithubPages";
    private const string BuildLocation = "Build/WebGLGithubPages";

    [MenuItem("Build/Build WebGL")]
    public static void BuildWebGL() {
        BuildProfile buildProfile = null;

        string[] guids = AssetDatabase.FindAssets("t:BuildProfile");

        if (guids.Length > 0) {
            foreach (string guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Debug.Log("BuildProfile asset found at path: " + path);
                BuildProfile p = AssetDatabase.LoadAssetAtPath<BuildProfile>(path);
                if (path.Contains(WebglGithubPages)) {
                    buildProfile = p;
                }
            }
        }

        if (buildProfile == null) {
            throw new Exception("No such build profile");
        }

        BuildProfile.SetActiveBuildProfile(buildProfile);
        BuildPlayerWithProfileOptions buildPlayerWithProfileOptions = new() {
            buildProfile = buildProfile,
            locationPathName = BuildLocation
        };

        BuildPipeline.BuildPlayer(buildPlayerWithProfileOptions);

        Debug.Log("WebGL Build Complete");
    }
}