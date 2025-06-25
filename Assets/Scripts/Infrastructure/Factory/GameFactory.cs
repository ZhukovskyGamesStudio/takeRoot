using UnityEngine;

public class GameFactory : IGameFactory
{
    private readonly IAssetProvider _assetProvider;

    public GameFactory(IAssetProvider assetProvider)
    {
        _assetProvider = assetProvider;
    }
    public GameObject CreateSettler(string settlerTypeId, Vector3 at)
    {
       return _assetProvider.Instantiate($"{AssetPath.SettlersPath}{settlerTypeId}", at);
    }

    public GameObject CreateResource(string resourceTypeId, Vector3 at, int amount)
    {
        return _assetProvider.Instantiate($"{AssetPath.ResourcesPath}{resourceTypeId}", at);
    }
}