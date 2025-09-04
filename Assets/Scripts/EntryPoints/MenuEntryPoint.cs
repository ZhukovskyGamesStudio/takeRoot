using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEntryPoint : EntryPointBase {
    public static MenuEntryPoint Instance;

    [SerializeField]
    private string _tgLink = "https://t.me/takeroot_pub";

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

    public void OpenTG() {
        Application.OpenURL(_tgLink);
    }

    public void Exit() {
        Application.Quit();
    }
}