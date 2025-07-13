using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Services;
using Unity.Cinemachine;
using UnityEngine;

public class EntryPoint_Video1 : MonoBehaviour, ICoroutineRunner {

	
	public SmoothCameraFollow2D CameraFollow;
	public CinemachineCamera CinemachineCamera;
	[SerializeField]
	private Camera _lastCamera;
	
	private ServiceLocator _services;

	public CommandTarget flower;
	public CommandTarget flower2;


	public Transform unZoomPos;
	public Transform swapPos;
	public Transform unZoomPos2;
	public Transform endPos;

	public Worker mainWorker;

	public GameObject mainView;
	public GameObject remboView;

	[SerializeField] private float _craftingUnzoom = 4.5f;

	[SerializeField] private float _craftingUnzoomSpeed = 0.025f, _lastUnzoomSpeed = 0.05f, _lastToCenterSpeed = 0.03f;

	public bool CanUnZoomCamera, CraftingZoom;
	public float CameraSize;

	private Vector2 CenterCamerav2 => new Vector2(_lastCamera.transform.position.x, _lastCamera.transform.position.y);

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
		_services.Single<IWorkerAssigner>().RegisterWorker(mainWorker);

		CreateWaterCommand(flower, 1).onComplete += () => CreateMoveCommand(unZoomPos, 2).onComplete += () =>
		{
			CraftingZoom = true;
			CreateWaterCommand(flower2, 3).onComplete += () => StartCoroutine(JumpAndChangeMood());
		};
		yield break;
	}

	private void LateUpdate() {
		if (CraftingZoom && CameraSize < _craftingUnzoom) {
			CameraSize += _craftingUnzoomSpeed;
			CinemachineCamera.Lens.OrthographicSize = CameraSize;
		}
		if (CanUnZoomCamera && CameraSize < _lastCamera.orthographicSize) {
			CameraSize += _lastUnzoomSpeed;
			CinemachineCamera.Lens.OrthographicSize = CameraSize;
			{
				CameraFollow.target = null;
				var curVec = new Vector2(CinemachineCamera.transform.position.x, CinemachineCamera.transform.position.y);
				var newPos = Vector2.MoveTowards(curVec, CenterCamerav2, _lastToCenterSpeed);
				CinemachineCamera.transform.position =
					new Vector3(newPos.x, newPos.y, CinemachineCamera.transform.position.z);
			}
		}
	}

	private BaseCommand CreateMoveCommand(Transform pos, int id) {
		return
			new MoveToCommand(id, pos.position, _services.Single<ICommandService>(), _services.Single<IUpdateService>(), mainWorker);
	}

	private BaseCommand CreateWaterCommand(CommandTarget target, int id) {
		return new WaterCommand(id, target, _services.Single<ICommandService>(), _services.Single<IUpdateService>(),
			mainWorker);
	}

	private IEnumerator JumpAndChangeMood() {
		//yield return WaitUntilAnimationEnds(mainView.GetComponent<Animator>(), "Jump");
		Action<AnimatorState> handler = null;
		handler = (state) =>
		{
			if (state != AnimatorState.Jump) return;
			mainWorker.Mover.SetMoveTime(0.5f);
			mainView.GetComponentInChildren<ChangeMoodAnimator>().currentMood = Mood.Angry;
			CreateMoveCommand(swapPos, 4).onComplete += () =>
			{
				SwapToCombat();
				CreateMoveCommand(unZoomPos2, 5).onComplete += () =>
				{
					CinemachineCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D = null;
					CreateMoveCommand(endPos, 6).onComplete += () =>
					{
						remboView.GetComponentInChildren<Shooter>().EnableShooting = true;
						CanUnZoomCamera = true;
					};
				};
			};
			mainWorker.WorkerAnimator.StateExited -= handler;
		};
		mainWorker.WorkerAnimator.StateExited += handler;
		mainWorker.WorkerAnimator.DoJump();
		yield break;
	}
	

	private void SwapToCombat() {
		mainView.SetActive(false);
		remboView.SetActive(true);
		mainWorker.WorkerAnimator = remboView.GetComponent<WorkerAnimator>();
		remboView.GetComponentInChildren<Shooter>().EnableShooting = false;
	}
	
	private IEnumerator WaitUntilAnimationEnds(Animator animator, string trigger)
	{
		animator.SetInteger("Action", 99);
		mainWorker.WorkerAnimator.DoJump();
		yield return null;
        

		yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(trigger) &&
		                                 animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
		animator.SetInteger("Action", 0);
	}
}