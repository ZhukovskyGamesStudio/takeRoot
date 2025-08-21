using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingEntryPoint : MonoBehaviour {
    public static bool LoadingSceneVisited = false;

    public void Start() {
        LoadingSceneVisited = true;
        SceneManager.LoadScene("MenuScene");
    }
}