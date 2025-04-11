using System;
using System.Collections;
using System.Collections.Generic;
using Settlers.Crafting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Video1 : MonoBehaviour
{
    public Settler flowerSettler;
    public Interactable firstFlower;
    public Interactable secondFlower;
    public bool firstCommandAdded;
    public bool secondCommandAdded;
    
    public List<Settler> craftingSettlers;
    public CraftingStationable craftingStation;
    public bool craftingCommandsAdded;

    public bool runIntoCraftingRoom;
    public Transform craftingRoom;
    public Transform craftingRoomCameraStopPos;
    public Transform craftingRoomCameraStartPos;
    public bool craftingRoomCameraStopped;
    public bool craftingRoomCameraStarted;

    public Transform goToRemboPos;
    public Transform changeToRemboPos;
    public Settler remboPrefab;
    public bool remboAdded;
    public bool goToRembo;

    public Transform endPos;
    public bool endPosAdded;

    public Transform startZoomPos;
    public SmoothCameraFollow2D CameraFollow;
    public bool CanUnZoomCamera;
    public float CameraSize;
    public Transform CenterCamera;
    private Vector2 CenterCamerav2 => new Vector2(CenterCamera.position.x, CenterCamera.position.y);


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftAlt)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (!craftingCommandsAdded)
        {
            craftingStation.AddRecipeToCraft("3");
            craftingCommandsAdded = true;
        }

        if (flowerSettler.TakenCommand == null)
        {
            var commandsManager = Core.CommandsManagersHolder.GetCommandManagerByRace(Race.Plants);
            if (!firstCommandAdded)
            {
                var firstCommand = new CommandData()
                {
                    Settler = flowerSettler,
                    CommandType = Command.Search,
                    Interactable = firstFlower
                };
                commandsManager.AddSubsequentCommand(firstCommand);
                firstCommandAdded = true;
                return;
            }

            if (!secondCommandAdded)
            {
                var secondCommand = new CommandData()
                {
                    Settler = flowerSettler,
                    CommandType = Command.Search,
                    Interactable = secondFlower
                };
                commandsManager.AddSubsequentCommand(secondCommand);
                secondCommandAdded = true;
                return;
            }
        }

        if (flowerSettler.TakenCommand == null && firstCommandAdded && secondCommandAdded)
        {
            flowerSettler.SettlerData._mood = Mood.Angry;
        }
        if (VectorUtils.ToVector2Int(flowerSettler.transform.position)
            == VectorUtils.ToVector2Int(goToRemboPos.position) && !goToRembo && flowerSettler.TakenCommand == null)
        {
            var tactCommand = new TacticalCommandData()
            {
                Settler = flowerSettler,
                TacticalCommandType = TacticalCommand.Move,
                TargetPosition = VectorUtils.ToVector2Int(changeToRemboPos.position)
            };
            flowerSettler.SettlerData._mode = Mode.Tactical;
            flowerSettler.SetTacticalCommand(tactCommand);
            goToRembo = true;
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
        if (VectorUtils.ToVector2Int(flowerSettler.transform.position)
            == VectorUtils.ToVector2Int(craftingRoom.position) && !runIntoCraftingRoom)
        {
            runIntoCraftingRoom = true;
        }
        if (VectorUtils.ToVector2Int(flowerSettler.transform.position)
            == VectorUtils.ToVector2Int(startZoomPos.position))
        {
            CanUnZoomCamera = true;
        }
        if (VectorUtils.ToVector2Int(flowerSettler.transform.position) ==
            VectorUtils.ToVector2Int(changeToRemboPos.position) && !remboAdded)
        {
            var rembo = Instantiate(remboPrefab, changeToRemboPos.position, Quaternion.identity);
            Destroy(flowerSettler.gameObject);
            flowerSettler = rembo.GetComponent<Settler>();
            CameraFollow.target = rembo.transform;
            remboAdded = true;
        }
        
        if (runIntoCraftingRoom && CameraSize < 5)
        {
            CameraSize += 0.05f;
            Camera.main.orthographicSize = CameraSize;
        }
        if (CameraSize < 36 && CanUnZoomCamera)
        {
            CameraSize += 0.09f;
            Camera.main.orthographicSize = CameraSize;
        }

        if (CameraSize > 7)
        {
            CameraFollow.target = null;
            float speed = 0.1f;
            var newPos = Vector2.MoveTowards(new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y), CenterCamerav2, speed);
            Camera.main.transform.position = new Vector3(newPos.x, newPos.y, Camera.main.transform.position.z);
        }
        if (remboAdded && !endPosAdded)
        {
            var tactCommand = new TacticalCommandData()
            {
                Settler = flowerSettler,
                TacticalCommandType = TacticalCommand.Move,
                TargetPosition = VectorUtils.ToVector2Int(endPos.position)
            };
            flowerSettler.SettlerData._mode = Mode.Tactical;
            flowerSettler.SetTacticalCommand(tactCommand);
            endPosAdded = true;
        }
    }
    
}