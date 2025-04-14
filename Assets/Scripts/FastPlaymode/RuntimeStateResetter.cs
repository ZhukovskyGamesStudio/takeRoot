using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class RuntimeStateResetter : MonoBehaviour {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init() {
        Core.Instance?.Reset();
    }
}