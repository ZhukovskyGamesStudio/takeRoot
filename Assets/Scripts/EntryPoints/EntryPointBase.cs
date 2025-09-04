using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryPointBase : NetworkBehaviour {
    protected bool TrySwitchToLoading() {
        if (!LoadingEntryPoint.LoadingSceneVisited) {
            SceneManager.LoadScene("LoadingScene");
            return true;
        }

        return false;
    }
}