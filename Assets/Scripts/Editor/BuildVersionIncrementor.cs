using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildVersionIncrementor : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        int buildNumber = PlayerSettings.iOS.buildNumber != "" 
            ? int.Parse(PlayerSettings.iOS.buildNumber) 
            : 0;

        buildNumber++;

#if UNITY_IOS
        PlayerSettings.iOS.buildNumber = buildNumber.ToString();
#elif UNITY_ANDROID
        PlayerSettings.Android.bundleVersionCode = buildNumber;
#else
        string[] versionParts = PlayerSettings.bundleVersion.Split('.');
        if (versionParts.Length == 3)
        {
            int patch = int.Parse(versionParts[2]) + 1;
            PlayerSettings.bundleVersion = $"{versionParts[0]}.{versionParts[1]}.{patch}";
        }
#endif

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}