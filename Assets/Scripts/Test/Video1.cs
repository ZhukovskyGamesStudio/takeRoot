using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Video1 : MonoBehaviour {
    
    public CinemachineCamera CinemachineCamera;
    public Settler flowerSettler;
    public Interactable firstFlower;
    public Interactable secondFlower;

    public List<Settler> craftingSettlers;
    public CraftingStationable craftingStation;

    [SerializeField]
    private Camera _lastCamera;

    public bool runIntoCraftingRoom;
    public Transform craftingRoom;
    
    public Transform changeToRemboPos;
    public Settler remboPrefab;


    public Transform removeCameraObstaclePos;
    public Transform startZoomPos;
    public SmoothCameraFollow2D CameraFollow;
    public bool CanUnZoomCamera;
    public float CameraSize;
    private Vector2 CenterCamerav2 => new Vector2(_lastCamera.transform.position.x, _lastCamera.transform.position.y);

    [SerializeField]
    private float _craftingUnzoom = 4.5f;

    [SerializeField]
    private float _craftingUnzoomSpeed = 0.025f, _lastUnzoomSpeed = 0.05f, _lastToCenterSpeed = 0.03f;

    [SerializeField]
    private List<Transform> _movePoses;

    private void Start() {
        PrepareScene();
        StartCoroutine(MainCoroutine());
    }

    private void PrepareScene() {
        craftingStation.AddRecipeToCraft("3");
    }

    private IEnumerator MainCoroutine() {
        yield return new WaitForSeconds(0.5f);
        yield return AddCommandAndWaitFinish(new CommandData() {
            Settler = flowerSettler,
            CommandType = Command.Search,
            Interactable = firstFlower
        });
        yield return AddCommandAndWaitFinish(new CommandData() {
            Settler = flowerSettler,
            CommandType = Command.Search,
            Interactable = secondFlower
        });
        //TODO Add jump
        var animator = flowerSettler.GetComponentInChildren<Animator>();
        yield return WaitUntilAnimationEnds(animator, "Jump");
        flowerSettler.SettlerData._mood = Mood.Angry;
        yield return AddMoveAndWaitFinish(_movePoses[0].position);
        SwapToRembo();
        yield return AddMoveAndWaitFinish(_movePoses[1].position);
        yield return AddMoveAndWaitFinish(_movePoses[2].position);
        yield return AddMoveAndWaitFinish(_movePoses[3].position);
        yield return AddMoveAndWaitFinish(_movePoses[4].position);
    }

    private IEnumerator WaitUntilAnimationEnds(Animator animator, string trigger)
    {
        animator.SetInteger("Action", 99);
        yield return null;
        
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(trigger) &&
                                   animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        animator.SetInteger("Action", 0);
    }
    private IEnumerator AddCommandAndWaitFinish(CommandData data) {
        var settler = data.Settler;
        CommandsManager commandsManager = Core.CommandsManagersHolder.GetCommandManagerByRace(Race.Plants);
        commandsManager.AddSubsequentCommand(data);
        yield return new WaitWhile(() => settler.TakenCommand != null);
    }

    private IEnumerator AddMoveAndWaitFinish(Vector3 newPos) {
        yield return StartCoroutine(AddCommandAndWaitFinish(new TacticalCommandData() {
            Settler = flowerSettler,
            TacticalCommandType = TacticalCommand.Move,
            TargetPosition = newPos.ToVector2Int()
        }));
    }

    private IEnumerator AddCommandAndWaitFinish(TacticalCommandData data) {
        var settler = data.Settler;
        settler.SettlerData._mode = Mode.Tactical;
        settler.SetTacticalCommand(data);
        yield return new WaitWhile(() => settler.TakenTacticalCommand != null);
    }

    private void Update() {
        CheckReloadSceneKeycode();
    }

    private void LateUpdate() {
        //if (VectorUtils.ToVector2Int(flowerSettler.transform.position)
        //    == VectorUtils.ToVector2Int(craftingRoomCameraStopPos.position) && !craftingRoomCameraStopped)
        //{
        //    CameraFollow.target = null;
        //    craftingRoomCameraStopped = true;
        //}        if (VectorUtils.ToVector2Int(flowerSettler.transform.position)
        //    == VectorUtils.ToVector2Int(craftingRoomCameraStartPos.position) && craftingRoomCameraStopped && !craftingRoomCameraStarted)
        //{
        //    CameraFollow.target = flowerSettler.gameObject.transform;
        //    craftingRoomCameraStarted = true;
        //}
        
        
        if (flowerSettler.transform.position.ToVector2Int() == craftingRoom.position.ToVector2Int() &&
            !runIntoCraftingRoom) {
            runIntoCraftingRoom = true;
        }

        if (flowerSettler.transform.position.ToVector2Int() == startZoomPos.position.ToVector2Int()) {
            CanUnZoomCamera = true;
        }

        if (runIntoCraftingRoom && CameraSize < _craftingUnzoom) {
            CameraSize += _craftingUnzoomSpeed;
            Camera.main.orthographicSize = CameraSize;
        }

        if (flowerSettler.transform.position.ToVector2Int() == removeCameraObstaclePos.position.ToVector2Int())
        {
            CinemachineCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D = null;
        }
        
        if (CanUnZoomCamera && CameraSize < _lastCamera.orthographicSize) {
            CameraSize += _lastUnzoomSpeed;
            CinemachineCamera.Lens.OrthographicSize = CameraSize;
            {
                CameraFollow.target = null;
                var curVec = new Vector2(CinemachineCamera.transform.position.x, CinemachineCamera.transform.position.y);
                var newPos = Vector2.MoveTowards(curVec, CenterCamerav2, _lastToCenterSpeed);
                CinemachineCamera.transform.position = new Vector3(newPos.x, newPos.y, CinemachineCamera.transform.position.z);
            }
        }
    }

    private static void CheckReloadSceneKeycode() {
        if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftAlt)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        //TODO add keycodes to speed up/down timescale
        //TODO /2 =1 *2
    }

    private void SwapToRembo() {
        var rembo = Instantiate(remboPrefab, changeToRemboPos.position, Quaternion.identity);
        Destroy(flowerSettler.gameObject);
        flowerSettler = rembo.GetComponent<Settler>();
        CameraFollow.target = rembo.transform;
        CinemachineCamera.Target.TrackingTarget = rembo.transform;
    }
}