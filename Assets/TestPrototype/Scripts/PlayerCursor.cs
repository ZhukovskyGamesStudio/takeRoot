using Unity.Netcode;
using UnityEngine;

public class PlayerCursor : NetworkBehaviour {
    [SerializeField]
    private RectTransform _rectTransform;
    private readonly NetworkVariable<CursorNetworkData> _netData = new(writePerm: NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn() {
        GameObject holder = GameObject.Find("CursorHolder");
        if(holder!= null) {
            _rectTransform.SetParent(GameObject.Find("CursorHolder").transform);
        }
    }

    private void Update() {
        if (IsOwner) {
            _rectTransform.position = Input.mousePosition;
            _netData.Value = new CursorNetworkData() {
                Pos = _rectTransform.position
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