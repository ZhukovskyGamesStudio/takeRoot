using System;
using CodeBase.Services;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
	private ServiceLocator _services;
	
	private void Awake() {
		var updateService = GetComponent<IUpdateService>();
		var loader = new ServiceLocatorLoader_Main(updateService);
		loader.RegisterServices();

		_services = ServiceLocator.Container;

		_services.Single<IDataProvider>().WorldResourcesData = new WorldResourcesData();
	}
}