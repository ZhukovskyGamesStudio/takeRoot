using UnityEngine;
using UnityEngine.SceneManagement;

public class Core : MonoBehaviour {
    public static Core Instance;
    public static CoreCanvasUi UI;

    private void Awake() {
        Instance = this;
        UI = FindAnyObjectByType<CoreCanvasUi>();

        UI.InitRace();
    }

    public void GoToMenu() {
        SceneManager.LoadScene("MenuScene");
    }
}