using System;
using CodeBase.Services;
using UnityEngine;

public class EntryPoint_Video1 : MonoBehaviour, ICoroutineRunner{
	private ServiceLocator _services;
	
	public Transform location1;
	public Transform location2;
	public Transform location3;

	public Worker mainWorker;
	
	bool Played = false;
	
	private void Awake() {
		_services = ServiceLocator.Container;
		var updateService = GetComponent<UpdateService>();
		var serviceLoader = new ServiceLocatorLoader_Main(updateService, this);
		serviceLoader.RegisterServices();
	}

	private void Update() {
		if (!Played) {
			PlayVideo();
			Played = true;
		}
	}

	private void PlayVideo() {
		_services.Single<IWorkerAssigner>().RegisterWorker(mainWorker);

		CreateMoveCommand(location1, 1).onComplete += () => CreateMoveCommand(location2, 2);

	}

	private BaseCommand CreateMoveCommand(Transform pos, int id) {
		return
			new MoveToCommand(id, pos.position, _services.Single<ICommandService>(), _services.Single<IUpdateService>(), mainWorker);
	}
}