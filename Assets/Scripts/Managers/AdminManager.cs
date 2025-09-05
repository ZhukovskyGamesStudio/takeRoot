using System.Linq;
using CodeBase.Services;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminManager : MonoBehaviour {
    [SerializeField]
    private GameObject _adminPanel;

    [SerializeField]
    private NetworkDataHolder _networkDataHolder;

    public static bool IsFakeOnline;
    public static bool IsInstaBuild = true;

    public static bool IsUnfogingDisabled = false;
    public static bool IsHumanBuildingsBuildable = false;
    public static bool FogHControls = true;

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
    
    public void SwitchInstaBuild(bool isOn) {
        IsInstaBuild = isOn;
    }
    
    public void SwitchUnfoging(bool isOn) {
        IsUnfogingDisabled = isOn;
    }
    
    public void SwitchHumanBuilds(bool isOn) {
        IsHumanBuildingsBuildable = isOn;
    }


    private Color _baseGreyColor;
    private bool _baseColorGot;
    
    public void SwitchGreyFog(bool isOn) {
        var res = FindObjectsByType<TilemapTypeData>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .FirstOrDefault(t => t.Type == TilemapType.FogOfWarGrey);

        if (res != null) {
            if (!_baseColorGot) {
                _baseColorGot = true;
                _baseGreyColor = res.Tilemap.color;
            }

            res.Tilemap.color = isOn ? _baseGreyColor : Color.clear;
        }
    }
    
    public void SwitchBlackFog(bool isOn) {
        var res = FindObjectsByType<TilemapTypeData>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .FirstOrDefault(t => t.Type == TilemapType.FogOfWarBlack);
        if (res != null) {
            res.Tilemap.color = isOn ? Color.white : Color.clear;
        }
    }

    public void SwitchCoreCanvasVisibility(bool isOn) {
        var res = GameObject.Find("CoreCanvas");
        if (res != null) {
            res.GetComponent<CanvasGroup>().alpha = isOn ? 0f : 1f;
        }
    }

    public void UnlockPause() {
        ServiceLocator.Container.Single<ITimeScaleService>().SetReadyToPause();
    }

    public void UnlockResearches() {
        ServiceLocator.Container.Single<IResearchService>().UnlockAllResearches();
    }

    public void StartFakeOnlineGame() {
        IsFakeOnline = true;
        if (NetworkDataHolder.Instance == null) {
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(_networkDataHolder.GetComponent<NetworkObject>());
            NetworkDataHolder.Instance.SelectRaceData.HostReady.Value = true;
            NetworkDataHolder.Instance.SelectRaceData.ClientReady.Value = true;
            NetworkDataHolder.Instance.MainGameNetworkData.HostRace.Value = Race.Plants;
            NetworkDataHolder.Instance.MainGameNetworkData.ClientRace.Value = Race.Robots;
        }

        SceneManager.LoadScene("CoreScene");
    }

    public void SwitchFakeRace() {
        IRaceService service = ServiceLocator.Container.Single<IRaceService>();
        service.RaceReactive.Value = service.RaceReactive.Value == Race.Plants ? Race.Robots : Race.Plants;
    }
}