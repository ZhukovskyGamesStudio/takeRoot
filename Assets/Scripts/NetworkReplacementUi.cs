using System;
using UnityEngine;

public class NetworkReplacementUi : MonoBehaviour {
    public Action<Race> OnChangeRace;

    public void ChangeRace() {
        Core.CurrentNetworkFakeRace = Core.CurrentNetworkFakeRace switch {
            Race.Plants => Race.Robots,
            Race.Robots => Race.Plants,
            _ => Core.CurrentNetworkFakeRace
        };
        OnChangeRace?.Invoke(Core.CurrentNetworkFakeRace);
    }
}