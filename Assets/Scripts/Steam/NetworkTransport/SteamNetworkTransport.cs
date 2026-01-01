using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Steamworks;

public class SteamNetworkTransport : NetworkTransport {
    public override ulong ServerClientId => _isHost ? SteamUser.GetSteamID().m_SteamID : _hostSteamId;

    private bool _isHost;
    private ulong _hostSteamId;
    private NetworkManager _networkManager;
    private readonly Dictionary<ulong, float> _clientRtts = new Dictionary<ulong, float>();
    private readonly Dictionary<ulong, float> _lastPacketTime = new Dictionary<ulong, float>();
    private readonly HashSet<ulong> _connectedClients = new HashSet<ulong>();
    private readonly Queue<NetworkEventData> _eventQueue = new Queue<NetworkEventData>();
    private bool _isInitialized;
    private const float RTT_UPDATE_INTERVAL = 0.5f;
    private const float CONNECTION_TIMEOUT = 10f;
    private float _lastRttUpdate;

    private struct NetworkEventData {
        public NetworkEvent Event;
        public ulong ClientId;
        public ArraySegment<byte> Payload;
        public float ReceiveTime;
    }

    public void SetHost(ulong hostId) {
        _hostSteamId = hostId;
    }

    public override void Initialize(NetworkManager networkManager) {
        if (!SteamManager.Initialized) {
            Debug.LogError("SteamManager not initialized!");
            return;
        }

        _networkManager = networkManager;
        _isInitialized = true;
        _connectedClients.Clear();
        _clientRtts.Clear();
        _lastPacketTime.Clear();
        _eventQueue.Clear();

        Callback<P2PSessionRequest_t>.Create(OnP2PSessionRequest);
        Callback<P2PSessionConnectFail_t>.Create(OnP2PSessionConnectFail);
    }

    private void OnP2PSessionRequest(P2PSessionRequest_t callback) {
        if (_isHost) {
            SteamNetworking.AcceptP2PSessionWithUser(callback.m_steamIDRemote);
            ulong clientId = callback.m_steamIDRemote.m_SteamID;
            if (!_connectedClients.Contains(clientId)) {
                _connectedClients.Add(clientId);
                _eventQueue.Enqueue(new NetworkEventData {
                    Event = NetworkEvent.Connect,
                    ClientId = clientId,
                    Payload = default,
                    ReceiveTime = Time.realtimeSinceStartup
                });
            }
        }
    }

    private void OnP2PSessionConnectFail(P2PSessionConnectFail_t callback) {
        ulong clientId = callback.m_steamIDRemote.m_SteamID;
        if (_connectedClients.Contains(clientId)) {
            _connectedClients.Remove(clientId);
            _eventQueue.Enqueue(new NetworkEventData {
                Event = NetworkEvent.Disconnect,
                ClientId = clientId,
                Payload = default,
                ReceiveTime = Time.realtimeSinceStartup
            });
        }
    }

    public override ulong GetCurrentRtt(ulong clientId) {
        if (_clientRtts.TryGetValue(clientId, out float rtt)) {
            return (ulong)rtt;
        }
        return 50;
    }

    public override void Shutdown() {
        if (!_isInitialized) {
            return;
        }

        foreach (ulong clientId in _connectedClients) {
            DisconnectRemoteClient(clientId);
        }

        _connectedClients.Clear();
        _clientRtts.Clear();
        _lastPacketTime.Clear();
        _eventQueue.Clear();
        _isInitialized = false;
    }

    public override bool StartClient() {
        if (!_isInitialized) {
            Debug.LogError("SteamNetworkTransport not initialized!");
            return false;
        }

        if (!SteamManager.Initialized) {
            Debug.LogError("SteamManager not initialized!");
            return false;
        }

        _isHost = false;

        if (_hostSteamId == 0) {
            Debug.LogError("Host Steam ID not set!");
            return false;
        }

        CSteamID hostSteamId = new CSteamID(_hostSteamId);
        bool sessionRequested = SteamNetworking.SendP2PPacket(hostSteamId, null, 0, EP2PSend.k_EP2PSendReliable);

        if (!sessionRequested) {
            Debug.LogError("Failed to request P2P session with host!");
            return false;
        }

        _connectedClients.Add(_hostSteamId);
        _eventQueue.Enqueue(new NetworkEventData {
            Event = NetworkEvent.Connect,
            ClientId = _hostSteamId,
            Payload = default,
            ReceiveTime = Time.realtimeSinceStartup
        });

        return true;
    }

    public override bool StartServer() {
        if (!_isInitialized) {
            Debug.LogError("SteamNetworkTransport not initialized!");
            return false;
        }

        if (!SteamManager.Initialized) {
            Debug.LogError("SteamManager not initialized!");
            return false;
        }

        _isHost = true;
        ulong localSteamId = SteamUser.GetSteamID().m_SteamID;
        _connectedClients.Add(localSteamId);

        return true;
    }

    public override void DisconnectLocalClient() {
        if (_isHost) {
            return;
        }

        if (_hostSteamId != 0) {
            DisconnectRemoteClient(_hostSteamId);
        }

        _connectedClients.Clear();
        _clientRtts.Clear();
        _lastPacketTime.Clear();
    }

    public override void DisconnectRemoteClient(ulong clientId) {
        if (!_connectedClients.Contains(clientId)) {
            return;
        }

        CSteamID steamId = new CSteamID(clientId);
        SteamNetworking.CloseP2PSessionWithUser(steamId);

        _connectedClients.Remove(clientId);
        _clientRtts.Remove(clientId);
        _lastPacketTime.Remove(clientId);

        if (!_isHost) {
            _eventQueue.Enqueue(new NetworkEventData {
                Event = NetworkEvent.Disconnect,
                ClientId = clientId,
                Payload = default,
                ReceiveTime = Time.realtimeSinceStartup
            });
        }
    }

    public override void Send(ulong clientId, ArraySegment<byte> payload, NetworkDelivery delivery) {
        if (!_isInitialized) {
            return;
        }

        if (!_connectedClients.Contains(clientId)) {
            return;
        }

        EP2PSend sendType = delivery == NetworkDelivery.Reliable 
            ? EP2PSend.k_EP2PSendReliable 
            : EP2PSend.k_EP2PSendUnreliable;

        CSteamID steamId = new CSteamID(clientId);
        bool success = SteamNetworking.SendP2PPacket(steamId, payload.Array, (uint)payload.Count, sendType);

        if (!success) {
            Debug.LogWarning($"Failed to send P2P packet to {clientId}");
        }
    }

    public override NetworkEvent PollEvent(out ulong clientId, out ArraySegment<byte> payload, out float receiveTime) {
        clientId = 0;
        payload = default;
        receiveTime = 0;

        if (!_isInitialized) {
            return NetworkEvent.Nothing;
        }

        if (_eventQueue.Count > 0) {
            NetworkEventData eventData = _eventQueue.Dequeue();
            clientId = eventData.ClientId;
            payload = eventData.Payload;
            receiveTime = eventData.ReceiveTime;
            return eventData.Event;
        }

        if (Time.time - _lastRttUpdate > RTT_UPDATE_INTERVAL) {
            UpdateRtts();
            _lastRttUpdate = Time.time;
        }

        CheckConnectionTimeouts();

        if (!SteamNetworking.IsP2PPacketAvailable(out uint size)) {
            return NetworkEvent.Nothing;
        }

        byte[] buffer = new byte[size];
        if (!SteamNetworking.ReadP2PPacket(buffer, size, out uint bytesRead, out CSteamID steamId)) {
            return NetworkEvent.Nothing;
        }

        ulong senderId = steamId.m_SteamID;

        if (!_connectedClients.Contains(senderId)) {
            if (_isHost) {
                SteamNetworking.AcceptP2PSessionWithUser(steamId);
                _connectedClients.Add(senderId);
                _eventQueue.Enqueue(new NetworkEventData {
                    Event = NetworkEvent.Connect,
                    ClientId = senderId,
                    Payload = default,
                    ReceiveTime = Time.realtimeSinceStartup
                });
            }
        }

        _lastPacketTime[senderId] = Time.realtimeSinceStartup;

        clientId = senderId;
        payload = new ArraySegment<byte>(buffer, 0, (int)bytesRead);
        receiveTime = Time.realtimeSinceStartup;

        return NetworkEvent.Data;
    }

    private void UpdateRtts() {
        if (!_isHost) {
            return;
        }

        foreach (ulong clientId in _connectedClients) {
            if (clientId == SteamUser.GetSteamID().m_SteamID) {
                continue;
            }

            if (!_clientRtts.ContainsKey(clientId)) {
                _clientRtts[clientId] = 50f;
            }
        }
    }

    private void CheckConnectionTimeouts() {
        if (!_isHost) {
            return;
        }

        float currentTime = Time.realtimeSinceStartup;
        List<ulong> clientsToRemove = new List<ulong>();

        foreach (ulong clientId in _connectedClients) {
            if (clientId == SteamUser.GetSteamID().m_SteamID) {
                continue;
            }

            if (_lastPacketTime.TryGetValue(clientId, out float lastTime)) {
                if (currentTime - lastTime > CONNECTION_TIMEOUT) {
                    clientsToRemove.Add(clientId);
                }
            } else {
                _lastPacketTime[clientId] = currentTime;
            }
        }

        foreach (ulong clientId in clientsToRemove) {
            _connectedClients.Remove(clientId);
            _clientRtts.Remove(clientId);
            _lastPacketTime.Remove(clientId);
            _eventQueue.Enqueue(new NetworkEventData {
                Event = NetworkEvent.Disconnect,
                ClientId = clientId,
                Payload = default,
                ReceiveTime = currentTime
            });
        }
    }
}