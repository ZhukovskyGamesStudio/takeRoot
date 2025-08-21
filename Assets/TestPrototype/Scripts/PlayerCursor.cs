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

        if (PlayerRaceSelection.IsCreated) {
            SubscribeToRaceChange();
        } else {
            PlayerRaceSelection.OnCreated += SubscribeToRaceChange;
        }
    }

    private void SubscribeToRaceChange() {
        Debug.Log("SubscribeToRaceChange");
        PlayerRaceSelection raceSelection = PlayerRaceSelection.Instance;
        raceSelection.Player1Race.OnValueChanged += (_, newValue) => {
            Debug.Log("Player1Race changed");
            if (OwnerClientId == 0) {
                SetCursorByRaceInternal(newValue);
            }
        };

        raceSelection.Player2Race.OnValueChanged += (_, newValue) => {
            Debug.Log("Player2Race changed");
            if (OwnerClientId == 1) {
                SetCursorByRaceInternal(newValue);
            }
        };
    }

    private void SetCursorByRaceInternal(Race race) {
        Debug.Log($"SetCursorByRaceInternal: {race} owner: {IsOwner} id: {OwnerClientId}");
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