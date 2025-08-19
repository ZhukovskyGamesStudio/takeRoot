using System;
using System.Collections.Generic;
using CodeBase.Services;
using UnityEngine;

public class GameFactory : IGameFactory {
	
	private readonly IAssetProvider _assetProvider;
	private readonly IDataProvider _dataProvider;
	private readonly IInputService _input;
	private readonly IPhysicsService _physics;
	private readonly IIdentifierService _identifier;


	public GameFactory(IAssetProvider assetProvider, IDataProvider dataProvider, IInputService input, IPhysicsService physics, ICoroutineRunner coroutineRunner, IPathfindService pathfinder, IIdentifierService identifier) {
		_assetProvider = assetProvider;
		_dataProvider = dataProvider;
		_input = input;
		_physics = physics;
		_identifier = identifier;
	}
	
	public GameObject CreateSettler(string settlerTypeId, Vector3 at) {
		var settler =  _assetProvider.Instantiate($"{AssetPath.SettlersPath}{settlerTypeId}", at);
		if (settler.TryGetComponent(out Worker worker)) {
		}
		return settler;
	}
	
	public GameObject CreateResource(string resourceTypeId, Vector3 at, int amount) {
		var resource = _assetProvider.Instantiate($"{AssetPath.ResourcesPath}{resourceTypeId}", at);
        
		var resourceView = resource.GetComponent<ResourceView>();
		resourceView.SetAmount(amount);
		
		_dataProvider.WorldResourcesData.ResourcesOnScene[new Vector2Int((int)resource.transform.position.x, (int)resource.transform.position.y)] = resourceView;
		return resource;
	}

	public Command CreateCommand(CommandType type) {
		throw new NotImplementedException();
	}
}