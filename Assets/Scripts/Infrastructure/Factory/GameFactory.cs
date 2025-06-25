using UnityEngine;

public class GameFactory : IGameFactory
{
	private readonly IAssetProvider _assetProvider;
	private readonly IDataProvider _dataProvider;
	
	
	public GameFactory(IAssetProvider assetProvider, IDataProvider dataProvider) {
		_assetProvider = assetProvider;
		_dataProvider = dataProvider;
	}
	
	public GameObject CreateSettler(string settlerTypeId, Vector3 at) {
		return _assetProvider.Instantiate($"{AssetPath.SettlersPath}{settlerTypeId}", at);
	}

	public GameObject CreateResource(string resourceTypeId, Vector3 at, int amount) {
		var resource = _assetProvider.Instantiate($"{AssetPath.ResourcesPath}{resourceTypeId}", at);
        
		var resourceView = resource.GetComponent<ResourceView>();
		resourceView.SetAmount(amount);
		
		_dataProvider.WorldResourcesData.ResourcesOnScene[new Vector2Int((int)resource.transform.position.x, (int)resource.transform.position.y)] = resourceView;
		return resource;
	}

}