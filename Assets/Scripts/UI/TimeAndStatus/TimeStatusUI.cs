using UnityEngine;

public class TimeStatusUI : MonoBehaviour {
    [field: SerializeField]
    public GameSpeedView GameSpeedView { get; private set; }

    [field: SerializeField]
    public DayStatusView DayStatusView { get; private set; }

    [field: SerializeField]
    public ColonyStatusView ColonyStatusView { get; private set; }

    public void Start() {
        GameSpeedView.SetFriendSelection(Random.Range(0, 2) == 1 ? GameSpeedType.High : GameSpeedType.Normal);
        DayStatusView.SetData(Random.Range(0, 99), Random.Range(0, 2) == 1);
        ColonyStatusView.SetData(Random.Range(0, 12), Random.Range(0, 101));
    }

    public void JumpToFriend() {
        Debug.LogWarning("Jump to Friend not implemented");
    }
}