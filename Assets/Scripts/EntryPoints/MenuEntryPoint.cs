using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEntryPoint : EntryPointBase {
    public static MenuEntryPoint Instance;

    [SerializeField]
    private OutsideLinksConfig _outsideLinksConfig;

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
        Application.OpenURL(_outsideLinksConfig.TgLink);
    }

    public void OpenSteam() {
        Application.OpenURL(_outsideLinksConfig.SteamLink);
    }

    public void OpenDiscord() {
        Application.OpenURL(_outsideLinksConfig.DiscordLink);
    }

    public void Exit() {
        Application.Quit();
    }
}