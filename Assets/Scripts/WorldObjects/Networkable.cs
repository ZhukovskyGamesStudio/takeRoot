using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Networkable : ECSComponent {
    public NetworkObject NetworkObject { get; private set; }

    public override int GetDependancyPriority() {
        return 0;
    }

    public override void Init(ECSEntity entity) {
        NetworkObject = GetComponent<NetworkObject>();
        if (NetworkManager.Singleton == null) {
            NetworkObject.enabled = false;
            NetworkObject.AutoObjectParentSync = false;
        }
    }

    public void ChangeParent(GameObject newParent) {
        if (NetworkManager.Singleton == null) {
            transform.SetParent(newParent.transform);
        } else {
            NetworkObject.TrySetParent(newParent);
        }
    }
}