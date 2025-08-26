using UnityEngine;

public class AssetProvider : IAssetProvider {
    public GameObject Instantiate(string path, Vector3 at) {
        GameObject prefab = Resources.Load<GameObject>(path);
        return Object.Instantiate(prefab, at, Quaternion.identity);
    }
}