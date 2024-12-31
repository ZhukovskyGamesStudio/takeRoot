using UnityEngine;

public class PlannedCommandView : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Sprite _searchIcon, _attackIcon;

    [SerializeField]
    private Transform _lb, _lt, _rt, _rb;

    public void Init(Command command, Vector3 pos, Vector2Int size) {
        SetIcon(command);
        transform.position = pos;
        SetSize(size);
    }

    private void SetIcon(Command command) {
        _spriteRenderer.sprite = command switch {
            Command.Search => _searchIcon,
            Command.Attack => _attackIcon,
            _ => _spriteRenderer.sprite
        };
    }

    private void SetSize(Vector2Int size) {
        _lb.localPosition = new Vector3(-size.x / 2f, -size.y / 2f);
        _lt.localPosition = new Vector3(-size.x / 2f, size.y / 2f);
        _rt.localPosition = new Vector3(size.x / 2f, size.y / 2f);
        _rb.localPosition = new Vector3(size.x / 2f, -size.y / 2f);
    }

    public void Release() {
        Destroy(gameObject);
    }
}