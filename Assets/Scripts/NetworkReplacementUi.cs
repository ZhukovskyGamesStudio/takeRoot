using System;
using UnityEngine;

public class NetworkReplacementUi : MonoBehaviour {
    public event Action<Race> OnChangeRace;

    public void ChangeRace() {
        Core.Instance.CurrentNetworkFakeRace = Core.Instance.CurrentNetworkFakeRace switch {
            Race.Plants => Race.Robots,
            Race.Robots => Race.Plants,
            _ => Core.Instance.CurrentNetworkFakeRace
        };
        OnChangeRace?.Invoke(Core.Instance.CurrentNetworkFakeRace);
    }
}