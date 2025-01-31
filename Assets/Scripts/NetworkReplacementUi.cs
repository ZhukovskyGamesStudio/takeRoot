using System;
using UnityEngine;

public class NetworkReplacementUi : MonoBehaviour {
    public Action<Race> OnChangeRace;

    public Race CurrentRace { get; private set; } = Race.Plants;

    public void ChangeRace() {
        CurrentRace = CurrentRace switch {
            Race.Plants => Race.Robots,
            Race.Robots => Race.Plants,
            _ => CurrentRace
        };
        OnChangeRace?.Invoke(CurrentRace);
    }
}