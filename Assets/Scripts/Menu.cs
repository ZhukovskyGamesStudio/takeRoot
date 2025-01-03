using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play() {
        SceneManager.LoadScene("CoreScene");
    }
}
