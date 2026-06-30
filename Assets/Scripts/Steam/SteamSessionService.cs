#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;
using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

// Steam matchmaking layer for the menu: creates/joins a Friends-only lobby, drives the overlay
// invite, and routes an accepted invite into PrepareGamePanel.JoinViaSteam(hostId). Lives on the
// persistent SteamManager GameObject so its callbacks survive scene loads and are pumped by
// SteamManager's RunCallbacks. The class always exists; its Steam guts are compiled out on
// non-standalone platforms (same idiom as SteamManager / SteamBoot).
//
// IMPORTANT: this service does NOT run its own duplicate-destroy guard. SteamManager already keeps
// exactly one of these GameObjects alive (it destroys duplicates in its own Awake). A second,
// independent guard here desynced with SteamManager's across the LoadingScene→MenuScene boot and
// could destroy the surviving GameObject's session, leaving Instance null. Instead we just cache
// ourselves and let the getter re-resolve to the live instance if the cache was on a culled
// duplicate.
[RequireComponent(typeof(SteamManager))]
public class SteamSessionService : MonoBehaviour {
    private static SteamSessionService _instance;

    public static SteamSessionService Instance {
        get {
            if (_instance == null) {
                _instance = FindFirstObjectByType<SteamSessionService>(FindObjectsInactive.Include);
            }
            return _instance;
        }
    }

    // Reset the cached static before entering Play Mode (covers disabled Domain Reload).
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() {
        _instance = null;
    }

#if !DISABLESTEAMWORKS
    private Callback<GameLobbyJoinRequested_t> _joinRequested;
    private Callback<LobbyEnter_t> _lobbyEntered;
    private CallResult<LobbyCreated_t> _lobbyCreated;
    private CSteamID _currentLobby;
#endif

    private void Awake() {
        _instance = this;
#if !DISABLESTEAMWORKS
        _joinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequested);
        _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        _lobbyCreated = CallResult<LobbyCreated_t>.Create(OnLobbyCreated);
#endif
    }

    private void Start() {
#if !DISABLESTEAMWORKS
        TryJoinFromLaunchArgs();
#endif
    }

    // Host: open a Friends-only lobby for 2 players, then invite via the overlay.
    public void HostLobby() {
#if !DISABLESTEAMWORKS
        if (!SteamManager.Initialized) {
            Debug.LogError("[Steam] HostLobby: SteamManager not initialized.");
            return;
        }
        _lobbyCreated.Set(SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2));
#endif
    }

    public void OpenInviteOverlay() {
#if !DISABLESTEAMWORKS
        if (_currentLobby.IsValid()) {
            SteamFriends.ActivateGameOverlayInviteDialog(_currentLobby);
        } else {
            Debug.LogWarning("[Steam] OpenInviteOverlay: no active lobby — host via Steam first.");
        }
#endif
    }

    public void LeaveLobby() {
#if !DISABLESTEAMWORKS
        if (_currentLobby.IsValid()) {
            SteamMatchmaking.LeaveLobby(_currentLobby);
            _currentLobby = CSteamID.Nil;
        }
#endif
    }

#if !DISABLESTEAMWORKS
    private void OnLobbyCreated(LobbyCreated_t cb, bool ioFailure) {
        if (ioFailure || cb.m_eResult != EResult.k_EResultOK) {
            Debug.LogError($"[Steam] CreateLobby failed: {(ioFailure ? "IO failure" : cb.m_eResult.ToString())}");
            return;
        }
        _currentLobby = new CSteamID(cb.m_ulSteamIDLobby);
        Debug.Log($"[Steam] Lobby created: {_currentLobby}. Opening the invite overlay…");
        OpenInviteOverlay();
    }

    // Friend accepted an invite / clicked "Join Game" while this game is already running.
    private void OnJoinRequested(GameLobbyJoinRequested_t cb) {
        Debug.Log($"[Steam] Join requested for lobby {cb.m_steamIDLobby}");
        SteamMatchmaking.JoinLobby(cb.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t cb) {
        _currentLobby = new CSteamID(cb.m_ulSteamIDLobby);
        CSteamID owner = SteamMatchmaking.GetLobbyOwner(_currentLobby);
        if (!owner.IsValid()) {
            Debug.LogError("[Steam] Entered lobby but its owner is invalid — aborting join.");
            return;
        }
        if (owner == SteamUser.GetSteamID()) {
            // We are the host entering our own freshly-created lobby — nothing to connect to.
            return;
        }

        Debug.Log($"[Steam] Entered lobby {_currentLobby}; host is {owner}. Connecting…");
        PrepareGamePanel panel = FindFirstObjectByType<PrepareGamePanel>(FindObjectsInactive.Include);
        if (panel != null) {
            panel.JoinViaSteam(owner.m_SteamID);
        } else {
            Debug.LogError("[Steam] Entered lobby but no PrepareGamePanel found to drive the join.");
        }
    }

    // Cold start: Steam launched us with "+connect_lobby <id>" after an invite was accepted while
    // the game was closed. Needs the real appid's "launch command line" partner setting to fire for
    // real (appid 480 can't fully validate this — see Phase 7).
    private void TryJoinFromLaunchArgs() {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++) {
            if (args[i] == "+connect_lobby" && ulong.TryParse(args[i + 1], out ulong lobbyId)) {
                Debug.Log($"[Steam] Cold-start join via +connect_lobby {lobbyId}");
                SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
                return;
            }
        }
    }
#endif

    private void OnDestroy() {
#if !DISABLESTEAMWORKS
        _joinRequested?.Dispose();
        _lobbyEntered?.Dispose();
        _lobbyCreated?.Dispose();
#endif
    }
}
