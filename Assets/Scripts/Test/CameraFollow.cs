using UnityEngine;

public class SmoothCameraFollow2D : MonoBehaviour {
    [Header("Target Settings")]
    public Transform target; // Объект, за которым следует камера

    public Vector2 offset = new Vector2(0f, 0f); // Смещение камеры относительно цели

    [Header("Smooth Settings")]
    public float smoothSpeed = 0.125f; // Скорость сглаживания (меньше = плавнее)

    public float lookAheadFactor = 0.5f; // Коэффициент опережения
    public float lookAheadReturnSpeed = 0.5f; // Скорость возврата опережения
    public float lookAheadMoveThreshold = 0.1f; // Порог движения для активации опережения

    private Vector3 _currentVelocity;
    private Vector3 _lookAheadPos;
    private float _targetZPosition; // Z-позиция камеры

    private void Start() {
        if (target == null) {
            Debug.LogWarning("Camera target not set! Please assign a target in the inspector.");
            return;
        }

        // Запоминаем начальную Z-позицию камеры
        _targetZPosition = transform.position.z;

        // Инициализируем камеру на позиции цели
        transform.position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, _targetZPosition);
    }

    private void LateUpdate() {
        if (target == null) return;

        // Вычисляем движение цели
        float xMoveDelta = (target.position - transform.position).x;

        // Обновляем опережение только если цель движется достаточно быстро
        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget) {
            _lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        } else {
            _lookAheadPos = Vector3.MoveTowards(_lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
        }

        // Целевая позиция камеры с учетом смещения и опережения
        Vector3 targetPosition = new Vector3(target.position.x + offset.x + _lookAheadPos.x, target.position.y + offset.y, _targetZPosition);

        // Плавное перемещение к целевой позиции
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothSpeed);
    }
}