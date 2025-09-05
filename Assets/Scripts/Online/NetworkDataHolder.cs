using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

public class NetworkDataHolder : NetworkBehaviour {
    public static NetworkDataHolder Instance;

    public static Action OnCreated;
    public static bool IsCreated = false;

    public SelectRaceNetworkData SelectRaceData;
    public MainGameNetworkData MainGameNetworkData;

    private void Awake() {
        Instance = this;
        IsCreated = true;
        DontDestroyOnLoad(gameObject);
        OnCreated?.Invoke();
    }

    public void ResetSelections() {
        MainGameNetworkData.HostRace.Value = Race.None;
        MainGameNetworkData.ClientRace.Value = Race.None;
        SelectRaceData.HostReady.Value = false;
        SelectRaceData.ClientReady.Value = false;
    }

    public bool IsHost => NetworkManager.Singleton.IsHost;

    [ServerRpc(RequireOwnership = false)]
    public void ChooseClientRaceServerRpc(int client, Race race) {
        (client == 1 ? MainGameNetworkData.HostRace : MainGameNetworkData.ClientRace).Value = race;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetClientReadyServerRpc(int client) {
        (client == 1 ? SelectRaceData.HostReady : SelectRaceData.ClientReady).Value = true;
    }

    public static Race GetRace() {
        return NetworkManager.Singleton.IsHost ? Instance.MainGameNetworkData.HostRace.Value : Instance.MainGameNetworkData.ClientRace.Value;
    }

    public static Race GetOtherRace() {
        return NetworkManager.Singleton.IsHost ? Instance.MainGameNetworkData.ClientRace.Value : Instance.MainGameNetworkData.HostRace.Value;
    }

    [ClientRpc]
    public void SetGameSpeedClientRpc(float speed) {
        Time.timeScale = speed;
    }

    [ClientRpc]
    public void SetSelectedTimeScaleClientRpc(GameSpeedType type, Race race) {
        ServiceLocator.Container.Single<ITimeScaleService>().SetTimeScaleOnly(type, race);
    }

    [ClientRpc]
    public void ClearAndCombineTilemapsClientRpc() {
        ClearAndCombineTilemaps();
    }

    public void ClearAndCombineTilemaps() {
        CombineTilemaps();
        ClearUnderTilemaps();
    }

    private static void CombineTilemaps() {
        List<TilemapTypeData> tilemaps = Object.FindObjectsByType<TilemapTypeData>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .ToList();

        var grouped = tilemaps.GroupBy(t => t.Type);

        foreach (var group in grouped) {
            TilemapTypeData mainData = group.FirstOrDefault(t => t.IsMain);
            if (mainData == null) {
                Debug.LogWarning($"No main Tilemap set for type {group.Key}, skipping.");
                continue;
            }

            Tilemap mainTilemap = mainData.Tilemap;

            foreach (var data in group) {
                if (data == mainData) continue;

                Tilemap tm = data.Tilemap;
                BoundsInt bounds = tm.cellBounds;

                foreach (Vector3Int localPos in bounds.allPositionsWithin) {
                    TileBase tile = tm.GetTile(localPos);
                    if (tile == null) continue;

                    // Мировая позиция тайла с учётом позиции тайлмапа
                    Vector3Int worldPos = localPos + new Vector3Int((int)tm.transform.position.x, (int)tm.transform.position.y, 0);
                    mainTilemap.SetTile(worldPos, tile);
                }

                tm.ClearAllTiles();
            }
        }
    }

    private static void ClearUnderTilemaps() {
        List<TilemapUnderCleaner> tilemapUnderCleaners =
            Object.FindObjectsByType<TilemapUnderCleaner>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        foreach (TilemapUnderCleaner tilemap in tilemapUnderCleaners) {
            tilemap.ClearUnderTilemap();
        }
    }

    [ClientRpc]
    public void GenerateRandomDecorClientRpc(float seed) {
        GenerateRandomDecor(seed);
    }

    public void GenerateRandomDecor(float seed) {
        List<RandomDecorObject> decors = Object.FindObjectsByType<RandomDecorObject>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .ToList();
        foreach (RandomDecorObject decor in decors) {
            decor.Init(seed);
        }
    }
}