using UnityEngine;

public class VideoTimeScaler : MonoBehaviour
{
    public void ChangeTimeScale(float value)
    {
        Time.timeScale += value;
    }

    public void RevertTimeScale()
    {
        Time.timeScale = 1;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
}