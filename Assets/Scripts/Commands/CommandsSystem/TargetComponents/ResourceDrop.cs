using UnityEngine;

public class ResourceDrop : MonoBehaviour {
    [Header("Resource Drop Settings")]
    public string resourceType = "Wood";

    public int dropAmount = 3;

    private Health _health;

    private void Start() {
        _health = GetComponent<Health>();
        if (_health != null) {
            _health.OnDeath += DropResource;
        } else {
            Debug.LogWarning($"ResourceDrop on {gameObject.name} requires Health component!");
        }
    }

    private void OnDestroy() {
        if (_health != null) {
            _health.OnDeath -= DropResource;
        }
    }

    private void DropResource() {
        Debug.Log($"Dropped {dropAmount} {resourceType} from {gameObject.name} at position {transform.position}");
        // Здесь будет реальная логика дропа ресурсов
        // Пока просто логируем
    }
}