using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
    public void Start() {
        SceneManager.LoadScene("MenuScene");
    }
}