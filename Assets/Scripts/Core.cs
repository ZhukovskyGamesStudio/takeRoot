using UnityEngine;
using UnityEngine.SceneManagement;

public class Core : MonoBehaviour {
    public void GoToMenu() {
        SceneManager.LoadScene("MenuScene");
    }
}