using System;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour {
    [SerializeField]
    [Range(0, 1f)]
    private float _multiplier = 0.5f;

    [SerializeField]
    private float _secondMultiplier = 0.2f;

    public static bool IsParallaxDisabled = false;
    
    private RectTransform _rectTransform;
    private Vector3 _initialPosition;

    private void Start() {
        _rectTransform = GetComponent<RectTransform>();
        _initialPosition = _rectTransform.position;
    }

    private void Update() {
        if (IsParallaxDisabled) {
            return;
        }

        var targetPosition = _initialPosition -
                             (Input.mousePosition - new Vector3(Screen.width, Screen.height) / 2) * _multiplier * _secondMultiplier;
        
        _rectTransform.position = Vector3.Lerp(_rectTransform.position, targetPosition, Time.deltaTime * 10f);
    }
}