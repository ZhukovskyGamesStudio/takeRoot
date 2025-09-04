using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCursor : NetworkBehaviour {
    [SerializeField]
    private RectTransform _rectTransform;

    [SerializeField]
    private Image _image;

    [SerializeField]
    private Sprite _undecidedCursor, _roboCursor, _plantsCursor;

    private readonly NetworkVariable<CursorNetworkData> _netData = new(writePerm: NetworkVariableWritePermission.Owner);
    private bool _isSubscribed;

    private void Update() {
        if (IsOwner) {
            _netData.Value = new CursorNetworkData {
                Pos = Input.mousePosition
            };
        } else {
            _rectTransform.position = _netData.Value.Pos;
        }
    }

    public override void OnNetworkSpawn() {
        CursorHolder holder = FindAnyObjectByType<CursorHolder>();
        if (holder != null) {
            _rectTransform.SetParent(holder.transform);
        }

        if (IsOwner) {
            _image.enabled = false;
        }

        if (NetworkDataHolder.IsCreated) {
            SubscribeToRaceChange();
        } else {
            NetworkDataHolder.OnCreated += SubscribeToRaceChange;
        }
    }

    private void SubscribeToRaceChange() {
        if (_isSubscribed) {
            return;
        }

        _isSubscribed = true;

        NetworkDataHolder raceSelection = NetworkDataHolder.Instance;
        raceSelection.MainGameNetworkData.HostRace.OnValueChanged += (_, newValue) => {
            if (OwnerClientId == 0) {
                SetCursorByRaceInternal(newValue);
            }
        };

        raceSelection.MainGameNetworkData.ClientRace.OnValueChanged += (_, newValue) => {
            if (OwnerClientId == 1) {
                SetCursorByRaceInternal(newValue);
            }
        };
    }

    private void SetCursorByRaceInternal(Race race) {
        if (IsOwner) {
            CursorManager.Instance.SetCursorByRace(race);
        } else {
            if (race == Race.None) {
                _image.sprite = _undecidedCursor;
            } else {
                _image.sprite = race == Race.Robots ? _roboCursor : _plantsCursor;
            }
        }
    }

    private struct CursorNetworkData : INetworkSerializable {
        public Vector3 Pos;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref Pos);
        }
    }
}