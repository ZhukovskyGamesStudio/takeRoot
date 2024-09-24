using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour {
    private NetworkVariable<Vector3> _netPos = new(writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Quaternion> _netRot = new(writePerm: NetworkVariableWritePermission.Owner);

    private NetworkVariable<PlayerNetworkData> _netData = new(writePerm: NetworkVariableWritePermission.Owner);

    private InputtedStringControl _playerController;
    [SerializeField]
    private PlayerHeadCanvas _playerHeadCanvas;

    private void Awake() {
        _playerController = GetComponent<InputtedStringControl>();
    }

    private void Update() {
        if (IsOwner) {
            _netPos.Value = transform.position;
            _netRot.Value = transform.rotation;
            _netData.Value = new PlayerNetworkData() {
                InputtedString = _playerController.InputtedString
            };
        } else {
            transform.position = _netPos.Value;
            transform.rotation = _netRot.Value;
            _playerController.InputtedString = _netData.Value.InputtedString.Value;
            _playerHeadCanvas.SetData(_netData.Value);
        }
    }

    public struct PlayerNetworkData : INetworkSerializable {
        public float HpPercent, EnergyPercent, BoostPercent;
        private FixedString64Bytes _inputtedString;

        public FixedString64Bytes InputtedString {
            get => _inputtedString;
            set => _inputtedString = value;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref _inputtedString);
            serializer.SerializeValue(ref HpPercent);
            serializer.SerializeValue(ref EnergyPercent);
            serializer.SerializeValue(ref BoostPercent);
        }
    }
}