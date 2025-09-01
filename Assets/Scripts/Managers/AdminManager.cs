using CodeBase.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminManager : MonoBehaviour {
    [SerializeField]
    private GameObject _adminPanel;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        _adminPanel.SetActive(false);
        AI.Settler.Immortal = true;
    }

    public void SwitchGodmode(bool isOn) {
        AI.Settler.GlobalGodmode = isOn;
    }
    public void SwitchImmortal(bool isOn) {
        AI.Settler.Immortal = isOn;
    }
    
    public void UnlockPause() {
        ServiceLocator.Container.Single<ITimeScaleService>().SetReadyToPause();
    }

    public void StartFakeOnlineGame() {
        SceneManager.LoadScene("CoreScene");
    }

    public void SwitchFakeRace() {
        IRaceService service = ServiceLocator.Container.Single<IRaceService>();
        service.RaceReactive.Value = service.RaceReactive.Value == Race.Plants ? Race.Robots : Race.Plants;
    }
}