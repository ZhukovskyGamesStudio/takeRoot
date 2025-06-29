using System;
using System.Collections.Generic;
using UnityEngine;

public class GameFactory : IGameFactory {
	
	private readonly IAssetProvider _assetProvider;
	private readonly IDataProvider _dataProvider;
	private readonly IInputService _input;
	private readonly IPhysicsService _physics;
	private readonly IIdentifierService _identifier;
	private readonly IWorkerService _workers;


	public GameFactory(IAssetProvider assetProvider, IDataProvider dataProvider, IInputService input, IPhysicsService physics, ICoroutineRunner coroutineRunner, IPathfindService pathfinder, IIdentifierService identifier, IWorkerService workers) {
		_assetProvider = assetProvider;
		_dataProvider = dataProvider;
		_input = input;
		_physics = physics;
		_identifier = identifier;
		_workers = workers;
	}
	
	public GameObject CreateSettler(string settlerTypeId, Vector3 at) {
		var settler =  _assetProvider.Instantiate($"{AssetPath.SettlersPath}{settlerTypeId}", at);
		if (settler.TryGetComponent(out Worker worker)) {
			_dataProvider.CreaturesData.CommandPerformers.Add(worker);
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

	public ICommand CreateCommand(CommandType type){
		switch (type) {
			case CommandType.Destroy:
				var target = _physics.Raycast<CommandTarget>(_input.GetWorldMousePosition(), Vector2.zero);
				return new DestroyCommand(_identifier.Next(), target, _workers);
			default:
				throw new ArgumentException($"Command type {type} is not supported for creation");
		}
	}
}