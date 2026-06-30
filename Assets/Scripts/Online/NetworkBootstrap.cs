#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
#if !DISABLESTEAMWORKS
using Netcode.Transports;
#endif

// The single place that knows about transports: it selects the NGO transport, applies the
// connect target, and starts the host/client. Menu-phase code (PrepareGamePanel, AdminManager)
// routes through here instead of touching NetworkManager.Singleton / UnityTransport directly.
public static class NetworkBootstrap {
    public static void StartHost(TransportKind kind) {
        SelectTransport(kind);
        NetworkManager.Singleton.StartHost();
    }

    public static void StartClient(ConnectTarget target) {
        SelectTransport(target.Kind);
        ApplyConnectTarget(target);
        NetworkManager.Singleton.StartClient();
    }

    private static void SelectTransport(TransportKind kind) {
        switch (kind) {
            case TransportKind.Unity:
                SetTransport(NetworkManager.Singleton.GetComponent<UnityTransport>());
                break;
            case TransportKind.Steam:
#if !DISABLESTEAMWORKS
                SetTransport(NetworkManager.Singleton.GetComponent<SteamNetworkingSocketsTransport>());
                break;
#else
                throw new NotSupportedException("Steam transport is unavailable on non-standalone platforms.");
#endif
            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
        }
    }

    private static void ApplyConnectTarget(ConnectTarget target) {
        switch (target.Kind) {
            case TransportKind.Unity:
                NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = target.Ip;
                break;
            case TransportKind.Steam:
#if !DISABLESTEAMWORKS
                NetworkManager.Singleton.GetComponent<SteamNetworkingSocketsTransport>().ConnectToSteamID = target.SteamId;
                break;
#else
                throw new NotSupportedException("Steam transport is unavailable on non-standalone platforms.");
#endif
            default:
                throw new ArgumentOutOfRangeException(nameof(target.Kind), target.Kind, null);
        }
    }

    private static void SetTransport(NetworkTransport transport) {
        NetworkManager.Singleton.NetworkConfig.NetworkTransport = transport;
    }

    // Host-side only: local IPv4 shown as the join code (UnityTransport / LAN path).
    public static string GetLocalIp() {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList) {
            if (ip.AddressFamily == AddressFamily.InterNetwork) {
                return ip.ToString();
            }
        }

        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}

public enum TransportKind {
    Unity,
    Steam
}

// Transport-neutral "where do I connect" descriptor. Unity uses Ip, Steam uses SteamId (host).
public struct ConnectTarget {
    public TransportKind Kind;
    public string Ip;
    public ulong SteamId;

    public static ConnectTarget ForIp(string ip) => new() { Kind = TransportKind.Unity, Ip = ip };
    public static ConnectTarget ForSteam(ulong steamId) => new() { Kind = TransportKind.Steam, SteamId = steamId };
}
