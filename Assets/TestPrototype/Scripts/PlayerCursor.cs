using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCursor : NetworkBehaviour {
    [SerializeField]
    private RectTransform _rectTransform;

    private readonly NetworkVariable<CursorNetworkData> _netData = new(writePerm: NetworkVariableWritePermission.Owner);

    [SerializeField]
    private PlayerRaceSelection _playerRaceSelection;

    [SerializeField]
    private Image _image;

    public override void OnNetworkSpawn() {
        CursorHolder holder = FindAnyObjectByType<CursorHolder>();
        if (holder != null) {
            _rectTransform.SetParent(holder.transform);
        }
    }

    private void Update() {
        if (IsOwner) {
            _image.enabled = false;
            _netData.Value = new CursorNetworkData() {
                Pos = Input.mousePosition
            };
        } else {
            _rectTransform.position = _netData.Value.Pos;
        }
    }

    private struct CursorNetworkData : INetworkSerializable {
        public Vector3 Pos;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref Pos);
        }
    }
}