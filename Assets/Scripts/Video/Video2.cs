using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class Video2 : MonoBehaviour
{
    public CinemachineCamera CinemachineCamera;

    public Settler mainLampSettler;
    public Settler chamomileSettler;
    [FormerlySerializedAs("potatoContainers")] public GameObject potatoContainer;
    [FormerlySerializedAs("rechargerPos")] [Header("Points")]
    public Transform goToLampRechargerPos;

    public Transform goToChamomileJumpPos;
    public Transform goToChamomileFarmPos;
    public Transform goToChamomileFarmPosFinal;
    public Transform goToChamomileGrinderPos;
    public Transform goToChamomileDistillerPos;
    public Transform goToChamomileGeneratorPos;
    public Transform goToChamomileRechargerPos;


    void Start()
    {
        StartCoroutine(MainCoroutine());
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
        StartCoroutine(SmoothZoom(7, 0.5f));
        yield return AddMoveAndWaitFinish(goToChamomileFarmPosFinal.position, chamomileSettler);
        yield return new WaitForSeconds(1f);
        yield return AddMoveAndWaitFinish(goToChamomileGrinderPos.position, chamomileSettler);
        yield return new WaitForSeconds(1f);
        yield return AddMoveAndWaitFinish(goToChamomileDistillerPos.position, chamomileSettler);
        yield return new WaitForSeconds(1f);
        yield return AddMoveAndWaitFinish(goToChamomileGeneratorPos.position, chamomileSettler);
        yield return new WaitForSeconds(1f);
        yield return AddMoveAndWaitFinish(goToChamomileRechargerPos.position, chamomileSettler);
        yield return new WaitForSeconds(1f);
    }


    private IEnumerator SmoothZoom(float targetZoomValue, float zoomSpeed)
    {
        while (CinemachineCamera.Lens.OrthographicSize < targetZoomValue)
        {
            CinemachineCamera.Lens.OrthographicSize += Time.deltaTime * zoomSpeed;
            yield return null;
        }
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