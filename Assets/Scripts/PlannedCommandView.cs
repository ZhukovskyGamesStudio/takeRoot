using UnityEngine;

public class PlannedCommandView : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Sprite _searchIcon, _breakIcon, _attackIcon, _storeIcon;

    [SerializeField]
    private SelectionView _selectionView;

    public void Init(Command command, Gridable gridable) {
        transform.position = gridable.GetCenterOnGrid;
        _selectionView.Init(gridable);
        SetIcon(command);
    }

    public void Init(TacticalCommand command, Gridable gridable) {
        transform.position = gridable.GetCenterOnGrid;
        _selectionView.Init(gridable);
        SetIcon(command);
    }

    private void SetIcon(Command command) {
        _spriteRenderer.sprite = command switch {
            Command.Search => _searchIcon,
            Command.Break => _breakIcon,
            Command.Transport => _storeIcon,
            Command.Store => _storeIcon,
            _ => _spriteRenderer.sprite
        };
    }

    private void SetIcon(TacticalCommand command) {
        _spriteRenderer.sprite = command switch {
            TacticalCommand.TacticalAttack => _attackIcon,
            _ => _spriteRenderer.sprite
        };
    }

    public void SetUnreachableState(bool isUnreachable) {
        SetColor(isUnreachable ? Color.grey : Color.white);
    }

    public void SetColor(Color color) {
        _spriteRenderer.color = color;
    }

    public void Release() {
        Destroy(gameObject);
    }
}