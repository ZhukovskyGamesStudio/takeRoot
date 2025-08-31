using UnityEngine;

public class MainMenuPanel : MonoBehaviour {
    [SerializeField]
    private Animation _animation;

    [SerializeField]
    private AnimationClip _show, _hide;

    public void Open() {
        BackgroundParallax.IsParallaxDisabled = false;
        //gameObject.SetActive(true);
        _animation.Play(_show.name);
    }

    public void Close() {
        BackgroundParallax.IsParallaxDisabled = true;
        //gameObject.SetActive(false);
        _animation.Play(_hide.name);
    }
}