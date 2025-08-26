using UnityEngine;

public interface IAssetProvider : IService {
    GameObject Instantiate(string path, Vector3 at);
}