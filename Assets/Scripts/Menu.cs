using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    public static Menu Instance;

    private void Awake() {
        Instance = this;
    }

    public void Play() {
        if (NetworkManager.Singleton.IsHost) {
            NetworkManager.Singleton.SceneManager.LoadScene("CoreScene", LoadSceneMode.Single);
        }
    }
}