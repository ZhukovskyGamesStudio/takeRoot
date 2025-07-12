using System.Collections;
using System.Collections.Generic;
using CodeBase.Services;
using Unity.Cinemachine;
using UnityEngine;

public class EntryPoint_Video2 : MonoBehaviour, ICoroutineRunner {

	public PowerManager powerManager;
	public CinemachineCamera CinemachineCamera;
	public GameObject potatoContainer;
	[Header("Characters")]
	public Worker toster;
	public Animator tosterAnimator;
	public Worker chamomile;

	[Header("Inventory")]
	public GameObject potato;
	public GameObject mash;
	public GameObject bottle;

	[Header("GameObjects")]
	public CommandTarget idleGenerator;
	public CommandTarget potatoField;
	public CommandTarget grinder;
	public CommandTarget distiller;
	public CommandTarget generator;
	public PowerConsumer bed;
	[Header("Move positions")]
	public Transform moveToBed;
	public Transform moveToBed2;
	public Transform moveToCornerOfTheRoad;
	public Transform moveToGrinder;
	public Transform moveToDistiller;
	public Transform moveToRoad2;
	public Transform moveToGenerator;
	public List<CommandTarget> wirePositions;

	private int _id;
	private ServiceLocator _services;
	bool Played = false;
	private void Awake() {
		_services = ServiceLocator.Container;
		var updateService = GetComponent<UpdateService>();
		var serviceLoader = new ServiceLocatorLoader_Main(updateService, this, GetComponent<MapFromSceneObjects>());
		serviceLoader.RegisterServices();
	}

	private void Update() {
		if (!Played) {
			StartCoroutine(PlayVideo());
			Played = true;
		}
	}

	private IEnumerator PlayVideo() {
		RandomPotato();
		idleGenerator.SetPerform(true);
		CreateMoveCommand(moveToBed, toster).onComplete += () =>
		{
			CreateMoveCommand(moveToBed2, chamomile).onComplete += () => CinemachineCamera.Follow = chamomile.transform;
		};
		yield return new WaitUntil(() => CinemachineCamera.Follow == chamomile.transform);
		yield return new WaitForSeconds(1.3f);
		CreateMoveCommand(moveToCornerOfTheRoad, chamomile)
			.onComplete += () => {
			StartCoroutine(CinemachineCamera.UnzoomCamera(7.3f, 0.015f));
			CreateSearchCommand(potatoField, chamomile).onComplete += () =>
			{
				potato.SetActive(true);
				CreateMoveCommand(moveToGrinder, chamomile).onComplete += () =>
				{
					potato.SetActive(false);
					CreateSearchCommand(grinder, chamomile).onComplete += () => mash.SetActive(true);
				};
				StartCoroutine(CinemachineCamera.ZoomCamera(3.5f, 0.025f));
			};
		};
		yield return new WaitUntil(() => mash.activeInHierarchy);
		CreateMoveCommand(moveToDistiller, chamomile).onComplete += () => {
			mash.SetActive(false);
			CreateSearchCommand(distiller, chamomile).onComplete += () => bottle.SetActive(true);
		};
		yield return new WaitUntil(() => bottle.activeInHierarchy);
		CreateMoveCommand(moveToRoad2, chamomile)
			.onComplete += () => CreateMoveCommand(moveToGenerator, chamomile)
			.onComplete += () => {
			bottle.SetActive(false);
			CreateSearchCommand(generator, chamomile).onComplete += () => {
				generator.UseAnimatorWhenPerform = true;
				generator.SetPerform(true);
			};
		};
		yield return new WaitUntil(() => generator.UseAnimatorWhenPerform);
		foreach (CommandTarget commandTarget in wirePositions) {
			CreateSearchCommand(commandTarget).onComplete += () => powerManager.CreateWireAt(commandTarget.transform.position.ToVector2Int());
		}

		yield return new WaitUntil(() => bed.Connected);
		yield return new WaitForSeconds(0.3f);
		CinemachineCamera.Follow = toster.transform;
		yield return new WaitForSeconds(1.5f);
		toster.transform.position = new Vector3(-11, 44.6f, 0);
		toster.transform.localScale = new Vector3(1, 1, 1);
		tosterAnimator.SetBool("IsSleeping", true);

	}
	
	private BaseCommand CreateMoveCommand(Transform pos, Worker worker) {
		return
			new MoveToCommand(_id++, pos.position, _services.Single<ICommandService>(), _services.Single<IUpdateService>(), worker);
	}	
	private BaseCommand CreateSearchCommand( CommandTarget commandTarget, Worker worker) {
		return
			new SearchCommand(_id++, commandTarget, _services.Single<ICommandService>(), _services.Single<IUpdateService>(), worker);
	}	
	private BaseCommand CreateSearchCommand( CommandTarget commandTarget) {
		return
			new SearchCommand(_id++, commandTarget, _services.Single<ICommandService>(), _services.Single<IUpdateService>());
	}
	private void RandomPotato()
	{
		foreach (Gridable potatoBed in potatoContainer.GetComponentsInChildren<Gridable>()) {
			var sprites = potatoBed.GetComponentInChildren<SpriteRenderer>().GetComponentsInChildren<Transform>(true);
			sprites[Random.Range(1, sprites.Length - 2)].gameObject.SetActive(true);
		}
	}
}