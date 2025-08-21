using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryPointBase : MonoBehaviour {

    protected bool TrySwitchToLoading() {
        if (!LoadingEntryPoint.LoadingSceneVisited) {
            SceneManager.LoadScene("LoadingScene");
            return true;
        }

        return false;
    }
}