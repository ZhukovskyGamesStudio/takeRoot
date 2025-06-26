using System;
using System.Collections.Generic;
using UnityEngine;

public class GameFactory : IGameFactory {
	private readonly Dictionary<Type, Func<ICommandParams, ICommand>> _commandCreators;
	
	private readonly IAssetProvider _assetProvider;
	private readonly IDataProvider _dataProvider;
	
	
	public GameFactory(IAssetProvider assetProvider, IDataProvider dataProvider, ICoroutineRunner coroutineRunner, IPathfindService pathfinder) {
		_assetProvider = assetProvider;
		_dataProvider = dataProvider;
		_commandCreators = new() {
			[typeof(DebugCommandParams)] = p => new DebugCommand((DebugCommandParams)p, coroutineRunner),
			[typeof(MoveCommandParams)] = p => new MoveCommand((MoveCommandParams)p, coroutineRunner, pathfinder),
			[typeof(DestroyCommandParams)] = p => new DestroyCommand((DestroyCommandParams)p, this),
		};
	}
	
	public GameObject CreateSettler(string settlerTypeId, Vector3 at) {
		var settler =  _assetProvider.Instantiate($"{AssetPath.SettlersPath}{settlerTypeId}", at);
		if (settler.TryGetComponent(out CommandPerformer performer)) {
			_dataProvider.CreaturesData.CommandPerformers.Add(performer);
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

	public ICommand CreateCommand<TParams>(TParams commandParams) where TParams : ICommandParams {
		if (_commandCreators.TryGetValue(typeof(TParams), out var creator)) {
			return creator(commandParams);
		}
		
		Debug.LogError($"Unknown command type: {typeof(TParams)}");
		return null;
	}
}