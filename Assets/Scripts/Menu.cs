using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    public static Menu Instance;
    [SerializeField]
    private NetworkManager _networkManager;

    private void Awake() {
        Instance = this;
    }

    public void Play() {
        _networkManager.SceneManager.LoadScene("CoreScene", LoadSceneMode.Single);
    }
}