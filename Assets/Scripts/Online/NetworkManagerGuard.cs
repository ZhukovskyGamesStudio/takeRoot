using Unity.Netcode;
using UnityEngine;

// The NetworkManager is a scene object in MenuScene, which the boot flow reloads
// (MenuScene → LoadingScene → MenuScene). NGO's NetworkManager calls DontDestroyOnLoad on itself
// in OnEnable, so the reload would otherwise leave a second, orphaned NetworkManager in the DDOL
// scene (NGO logs a "more than one NetworkManager" warning on play-stop). Running before
// NetworkManager (low execution order), this guard drops the duplicate GameObject before its
// NetworkManager awakes, so exactly one instance survives.
[DefaultExecutionOrder(-10000)]
[RequireComponent(typeof(NetworkManager))]
public class NetworkManagerGuard : MonoBehaviour {
    private void Awake() {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.gameObject != gameObject) {
            DestroyImmediate(gameObject);
        }
    }
}
