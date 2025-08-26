using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class RuntimeStateResetter : MonoBehaviour {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init() {
        ObsoleteCoreEntryPoint.Instance?.Reset();
        LoadingEntryPoint.LoadingSceneVisited = false;
    }
}