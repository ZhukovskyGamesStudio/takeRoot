using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MenuEntryPoint : EntryPointBase {
    public static MenuEntryPoint Instance;

    private void Awake() {
        if (TrySwitchToLoading()) {
            return;
        }

        Instance = this;
    }

    public void Play() {
        if (NetworkManager.Singleton.IsHost) {
            NetworkManager.Singleton.SceneManager.LoadScene("CoreScene", LoadSceneMode.Single);
        }
    }
}