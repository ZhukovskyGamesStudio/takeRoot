using System;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminManager : MonoBehaviour {
    [SerializeField]
    private GameObject _adminPanel;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        _adminPanel.SetActive(false);
    }

    public void StartFakeOnlineGame() {
        SceneManager.LoadScene("CoreScene");
    }

    public void SwitchFakeRace() {
        var service = ServiceLocator.Container.Single<IRaceService>();
        service.RaceReactive.Value = service.RaceReactive.Value == Race.Plants ? Race.Robots : Race.Plants;
    }
}