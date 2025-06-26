using System;
using CodeBase.Services;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
	private ServiceLocator _services;
	
	private void Awake() {
		var updateService = GetComponent<IUpdateService>();
		var coroutineRunner = GetComponent<ICoroutineRunner>();
		var loader = new ServiceLocatorLoader_Main(updateService, coroutineRunner);
		loader.RegisterServices();

		_services = ServiceLocator.Container;

		_services.Single<IDataProvider>().WorldResourcesData = new WorldResourcesData();
	}
}