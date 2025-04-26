using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Video2 : MonoBehaviour
{
    public CinemachineCamera CinemachineCamera;

    public Settler mainLampSettler;
    public Settler chamomileSettler;
    public GameObject potatoContainer;
    public GameObject potatoField;
    public Interactable distiller;
    public Interactable grinder;
    public Interactable biogenerator;
    [Header("Points")]
    public Transform goToLampRechargerPos;
    public Transform goToChamomileJumpPos;
    public Transform goToChamomileFarmPos;
    public Transform goToChamomileFarmPosFinal;
    public Transform goToChamomileGrinderPos;
    public Transform goToChamomileDistillerPos;
    public Transform goToChamomileGeneratorPos;
    public Transform goToChamomileRechargerPos;
    public Transform goToFarmUnzoomPos;
    public Transform goToGrinderZoomPos;
    public Transform goToGeneratorUnzoomPos;
    public Transform goToGeneratorZoomPos;
    
    [Header("Item")] 
    public SpriteRenderer itemPlaceholder;

    public Sprite potato;
    public Sprite potatoMash;
    public Sprite biofuel;

    private Coroutine _zoomCoroutine;
    void Start()
    {
        StartCoroutine(MainCoroutine());
    }

    private void LateUpdate()
    {
        if (chamomileSettler.transform.position.ToVector2Int() == goToFarmUnzoomPos.position.ToVector2Int())
            Zoom(7, 0.5f);
        if (chamomileSettler.transform.position.ToVector2Int() == goToGrinderZoomPos.position.ToVector2Int())
            Zoom(4, 0.5f);
        if (chamomileSettler.transform.position.ToVector2Int() == goToGeneratorUnzoomPos.position.ToVector2Int())
            Zoom(7, 0.5f);
        if (chamomileSettler.transform.position.ToVector2Int() == goToGeneratorZoomPos.position.ToVector2Int())
            Zoom(4, 0.5f);
    }

    private IEnumerator MainCoroutine()
    {
        RandomPotato();
        yield return new WaitForSeconds(0.5f);
        yield return AddMoveAndWaitFinish(goToLampRechargerPos.position, mainLampSettler);
        yield return new WaitForSeconds(0.5f);
        yield return AddMoveAndWaitFinish(goToChamomileJumpPos.position, chamomileSettler);
        CinemachineCamera.Target.TrackingTarget = chamomileSettler.transform;
        yield return new WaitForSeconds(1f);
        yield return AddMoveAndWaitFinish(goToChamomileFarmPos.position, chamomileSettler);
        Zoom(7, 0.5f);
        yield return AddMoveAndWaitFinish(goToChamomileFarmPosFinal.position, chamomileSettler);
        chamomileSettler.FakeCommand = Command.Craft;
        yield return new WaitForSeconds(1f);
        chamomileSettler.FakeCommand = Command.None;
        potatoField.SetActive(false);
        itemPlaceholder.sprite = potato;
        Zoom(4, 0.5f);
        yield return AddMoveAndWaitFinish(goToChamomileGrinderPos.position, chamomileSettler);
        grinder.Animator.SetTrigger("Work");
        yield return DoFakeCommandAndWaitFinish(chamomileSettler, Command.Craft, 2f);
        grinder.Animator.SetTrigger("Idle");
        itemPlaceholder.sprite = potatoMash;
        yield return AddMoveAndWaitFinish(goToChamomileDistillerPos.position, chamomileSettler);
        distiller.Animator.SetTrigger("Work");
        yield return DoFakeCommandAndWaitFinish(chamomileSettler, Command.Craft, 3f);
        distiller.Animator.SetTrigger("Idle");
        itemPlaceholder.sprite = biofuel;
        yield return AddMoveAndWaitFinish(goToChamomileGeneratorPos.position, chamomileSettler);
        yield return DoFakeCommandAndWaitFinish(chamomileSettler, Command.Craft, 1f);
        biogenerator.Animator.SetTrigger("Work");
        itemPlaceholder.sprite = null;
        yield return AddMoveAndWaitFinish(goToChamomileRechargerPos.position, chamomileSettler);
        yield return DoFakeCommandAndWaitFinish(chamomileSettler, Command.Craft, 1f);
    }
    

    private void Zoom(float targetZoomValue, float zoomSpeed)
    {
        if (_zoomCoroutine != null)
            StopCoroutine(_zoomCoroutine);
        if (CinemachineCamera.Lens.OrthographicSize < targetZoomValue)
            _zoomCoroutine = StartCoroutine(SmoothZoom(targetZoomValue, zoomSpeed));
        else
            _zoomCoroutine = StartCoroutine(SmoothUnzoom(targetZoomValue, zoomSpeed));
    }
    private IEnumerator SmoothZoom(float targetZoomValue, float zoomSpeed)
    {
        while (CinemachineCamera.Lens.OrthographicSize < targetZoomValue)
        {
            CinemachineCamera.Lens.OrthographicSize += Time.deltaTime * zoomSpeed;
            yield return null;
        }
    }
    private IEnumerator SmoothUnzoom(float targetZoomValue, float zoomSpeed)
    {
        while (CinemachineCamera.Lens.OrthographicSize > targetZoomValue)
        {
            CinemachineCamera.Lens.OrthographicSize -= Time.deltaTime * zoomSpeed;
            yield return null;
        }
    }

    private IEnumerator DoFakeCommandAndWaitFinish(Settler settler, Command command, float time)
    {
        settler.FakeCommand = command;
        yield return new WaitForSeconds(time);
        settler.FakeCommand = Command.None;
    }
    private IEnumerator AddMoveAndWaitFinish(Vector3 newPos, Settler settler) {
        yield return StartCoroutine(AddCommandAndWaitFinish(new TacticalCommandData() {
            Settler = settler,
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

    private void RandomPotato()
    {
        foreach (Gridable potatoBed in potatoContainer.GetComponentsInChildren<Gridable>())
        {
            var sprites = potatoBed.GetComponentInChildren<SpriteRenderer>().GetComponentsInChildren<Transform>(true);
            sprites[Random.Range(1, sprites.Length - 2)].gameObject.SetActive(true);
        }
    }
}