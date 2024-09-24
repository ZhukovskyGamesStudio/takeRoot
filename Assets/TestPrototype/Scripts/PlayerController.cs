using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : NetworkBehaviour {
    private NavMeshAgent _navMeshAgent;

    private Vector3 _moveTarget;

    [SerializeField]
    private Renderer _renderer;

    [SerializeField]
    private Material _tree, _robot;

    private void Awake() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 1000, LayerMask.GetMask("Ground"));
        _moveTarget = hitInfo.point;
    }

    //deletes this scripts on other clients if they are not owner, thus removing controls from them
    public override void OnNetworkSpawn() {
        if ((NetworkManager.IsHost && IsOwner) || (!NetworkManager.IsHost && !IsOwner)) {
            _renderer.material = _tree;
        } else {
            _renderer.material = _robot;
        }

        if (!IsOwner) {
            enabled = false;
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000, LayerMask.GetMask("Ground"))) {
                _moveTarget = hitInfo.point;
            } else {
                Debug.LogWarning("No ground hitted by ray");
            }

            _navMeshAgent.SetDestination(_moveTarget);
        }
    }
}