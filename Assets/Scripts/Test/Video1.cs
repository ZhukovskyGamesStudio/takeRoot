using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Video1 : MonoBehaviour {
    public Settler flowerSettler;
    public Interactable firstFlower;
    public Interactable secondFlower;

    public List<Settler> craftingSettlers;
    public CraftingStationable craftingStation;

    public bool runIntoCraftingRoom;
    public Transform craftingRoom;
    public Transform craftingRoomCameraStopPos;
    public Transform craftingRoomCameraStartPos;
    public bool craftingRoomCameraStopped;
    public bool craftingRoomCameraStarted;

    public Transform changeToRemboPos;
    public Settler remboPrefab;

    public Transform startZoomPos;
    public SmoothCameraFollow2D CameraFollow;
    public bool CanUnZoomCamera;
    public float CameraSize;
    public Transform CenterCamera;
    private Vector2 CenterCamerav2 => new Vector2(CenterCamera.position.x, CenterCamera.position.y);

    [SerializeField]
    private float _craftingUnzoom = 4.5f, _lastUnzoom = 32;

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
        flowerSettler.SettlerData._mood = Mood.Angry;
        yield return AddMoveAndWaitFinish(_movePoses[0].position);
        SwapToRembo();
        yield return AddMoveAndWaitFinish(_movePoses[1].position);
        yield return AddMoveAndWaitFinish(_movePoses[2].position);
        yield return AddMoveAndWaitFinish(_movePoses[3].position);
        yield return AddMoveAndWaitFinish(_movePoses[4].position);
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
            TargetPosition = VectorUtils.ToVector2Int(newPos)
        }));
    }

    private IEnumerator AddCommandAndWaitFinish(TacticalCommandData data) {
        var settler = data.Settler;
        settler.SettlerData._mode = Mode.Tactical;
        settler.SetTacticalCommand(data);
        yield return new WaitWhile(() => settler.TakenTacticalCommand != null);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftAlt)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

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
        if (VectorUtils.ToVector2Int(flowerSettler.transform.position) == VectorUtils.ToVector2Int(craftingRoom.position) &&
            !runIntoCraftingRoom) {
            runIntoCraftingRoom = true;
        }

        if (VectorUtils.ToVector2Int(flowerSettler.transform.position) == VectorUtils.ToVector2Int(startZoomPos.position)) {
            CanUnZoomCamera = true;
        }

        if (runIntoCraftingRoom && CameraSize < _craftingUnzoom) {
            CameraSize += _craftingUnzoomSpeed;
            Camera.main.orthographicSize = CameraSize;
        }

        if (CanUnZoomCamera && CameraSize < _lastUnzoom) {
            CameraSize += _lastUnzoomSpeed;
            Camera.main.orthographicSize = CameraSize;
            {
                CameraFollow.target = null;
                var curVec = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
                var newPos = Vector2.MoveTowards(curVec, CenterCamerav2, _lastToCenterSpeed);
                Camera.main.transform.position = new Vector3(newPos.x, newPos.y, Camera.main.transform.position.z);
            }
        }
    }

    private void SwapToRembo() {
        var rembo = Instantiate(remboPrefab, changeToRemboPos.position, Quaternion.identity);
        Destroy(flowerSettler.gameObject);
        flowerSettler = rembo.GetComponent<Settler>();
        CameraFollow.target = rembo.transform;
    }
}