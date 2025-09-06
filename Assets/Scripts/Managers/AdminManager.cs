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

    public void SpawnPlanks() {
        if (!_networkDataHolder.IsHost) {
            return;
        }
        var coord = GetCenterScreenPos();
        ServiceLocator.Container.Single<IResourceManager>().SpawnResource(coord, ResourceType.Planks, 100);
    }

    public void SpawnMetaScraps() {
        if (!_networkDataHolder.IsHost) {
            return;
        }
        var coord = GetCenterScreenPos();
        ServiceLocator.Container.Single<IResourceManager>().SpawnResource(coord, ResourceType.MetalScraps, 100);
    }

    public void SpawnDirt() {
        if (!_networkDataHolder.IsHost) {
            return;
        }
        var coord = GetCenterScreenPos();
        ServiceLocator.Container.Single<IResourceManager>().SpawnResource(coord, ResourceType.Dirt, 100);
    }

    public void SpawnArtiact() {
        if (!_networkDataHolder.IsHost) {
            return;
        }
        var coord = GetCenterScreenPos();
        ServiceLocator.Container.Single<IResourceManager>().SpawnResource(coord, ResourceType.Artifact, 1);
    }

    private static Vector3 GetCenterScreenPos() {
        var pos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
        pos.z = 0;
        return pos;
    }

    public void StartFakeOnlineGame() {
        IsFakeOnline = true;

        if (NetworkDataHolder.Instance == null) {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

            NetworkManager.Singleton.StartHost();
            // ничего не спавним и не трогаем RPC пока клиент не зарегистрируется
        }

        SceneManager.LoadScene("CoreScene");
    }

    private void OnClientConnected(ulong clientId) {
        if (clientId != NetworkManager.Singleton.LocalClientId) {
            return;
        }

        // локальный клиент (Host) подключился, теперь можно спавнить
        var obj = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(_networkDataHolder.GetComponent<NetworkObject>());

        NetworkDataHolder.Instance.SelectRaceData.HostReady.Value = true;
        NetworkDataHolder.Instance.SelectRaceData.ClientReady.Value = true;
        NetworkDataHolder.Instance.MainGameNetworkData.HostRace.Value = Race.Plants;
        NetworkDataHolder.Instance.MainGameNetworkData.ClientRace.Value = Race.Robots;

        Debug.Log("Fake online game initialized on host");

        // отписываемся, чтобы не вызывалось повторно
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    public void SwitchFakeRace() {
        IRaceService service = ServiceLocator.Container.Single<IRaceService>();
        service.RaceReactive.Value = service.RaceReactive.Value == Race.Plants ? Race.Robots : Race.Plants;
    }
}