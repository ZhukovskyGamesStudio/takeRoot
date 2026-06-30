#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

// Sits next to SteamManager on the menu boot GameObject. SteamManager.Awake() runs SteamAPI.Init();
// once that is up this warms the Steam Datagram Relay (so the first P2P connect isn't slow) and logs
// the local SteamID as a dev sanity signal. The class always exists so the scene never loses the
// component; its Steam guts are compiled out on non-standalone platforms, same idiom as SteamManager.
[RequireComponent(typeof(SteamManager))]
public class SteamBoot : MonoBehaviour {
    private void Start() {
#if !DISABLESTEAMWORKS
        if (!SteamManager.Initialized) {
            Debug.LogError("[Steam] SteamManager not initialized — is Steam running and are you logged in?");
            return;
        }

        SteamNetworkingUtils.InitRelayNetworkAccess();
        Debug.Log($"[Steam] Initialized. Local SteamID = {SteamUser.GetSteamID()} ({SteamFriends.GetPersonaName()})");
#endif
    }
}
