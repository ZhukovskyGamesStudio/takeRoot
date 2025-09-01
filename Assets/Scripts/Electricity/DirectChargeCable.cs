using UnityEngine;

public class DirectChargeCable : MonoBehaviour {
    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    private int _segments = 20;

    [SerializeField]
    private float _sagAmount = 0.5f;

    [SerializeField]
    private float _speed = 1f;

    [SerializeField]
    private float _pulseLength = 0.2f;

    private Transform _target;
    private float _progress;
    private Gradient _gradient;
    private GradientColorKey[] _colorKeys;
    private GradientAlphaKey[] _alphaKeys;

    public void Connect(AI.Settler settler) {
        gameObject.SetActive(true);
        _target = settler.transform;
    }

    public void Disconnect() {
        gameObject.SetActive(false);
        _target = null;
        _lineRenderer.positionCount = 0;
    }

    private void Awake() {
        _gradient = new Gradient();
        _colorKeys = new GradientColorKey[3];
        _colorKeys[0].color = Color.black;
        _colorKeys[0].time = 0f;
        _colorKeys[1].color = Color.yellow;
        _colorKeys[1].time = 0.5f;
        _colorKeys[2].color = Color.black;
        _colorKeys[2].time = 1f;

        _alphaKeys = new GradientAlphaKey[1];
        _alphaKeys[0].alpha = 1.0f;

        _gradient.SetKeys(_colorKeys, _alphaKeys);
    }

    private void Update() {
        if (_target == null) {
            return;
        }

        _lineRenderer.positionCount = _segments + 1;
        _lineRenderer.positionCount = _segments + 1;
        Vector3 start = transform.position;
        Vector3 end = _target.position;
        for (int i = 0; i <= _segments; i++) {
            float t = i / (float)_segments;
            Vector3 pos = GetCablePoint(start, end, t);
            _lineRenderer.SetPosition(i, pos);
        }
        _progress += Time.deltaTime * _speed;
        if (_progress > 1f) {
            _progress = 0f;
        }

        var keys = _lineRenderer.widthCurve.keys;

        float pulseStart = _progress - _pulseLength * 0.5f;
        float pulseEnd = _progress + _pulseLength * 0.5f;

        keys[1].time = pulseStart;
        _colorKeys[0].time = pulseStart;
        keys[2].time = _progress;
        _colorKeys[1].time = _progress;
        keys[3].time = pulseEnd;
        _colorKeys[2].time = pulseEnd;

        _lineRenderer.widthCurve = new AnimationCurve(keys);
        _lineRenderer.colorGradient = _gradient;
        _gradient.SetKeys(_colorKeys, _alphaKeys);
    }
    
    private Vector3 GetCablePoint(Vector3 start, Vector3 end, float t) {
        Vector3 pos = Vector3.Lerp(start, end, t);
        float sag = Mathf.Sin(t * Mathf.PI) * _sagAmount;
        pos.y -= sag;
        return pos;
    }
}