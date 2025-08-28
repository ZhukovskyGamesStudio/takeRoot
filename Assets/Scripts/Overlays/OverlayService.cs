using UnityEngine;

public class OverlayService : IOverlayService {
    public void ChangeOverlayTo(OverlayType type) {
        Debug.Log($"Overlay changed to {type}");
    }
}