using System.Collections.Generic;
using UnityEngine;

public class PlannedCommandView : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Sprite _searchIcon, _breakIcon, _attackIcon, _storeIcon;

    [SerializeField]
    private Transform _lb, _lt, _rt, _rb;

    public void Init(Command command, Gridable gridable) {
        transform.position = gridable.GetCenterOnGrid;
        SetSize(gridable.GetGridEdgePoints());
        SetIcon(command);
    }    
    public void Init(TacticalCommand command, Gridable gridable) {
        transform.position = gridable.GetCenterOnGrid;
        SetSize(gridable.GetGridEdgePoints());
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

    private void SetSize(List<Vector3> edgePoints) {
        _lb.position = edgePoints[0];
        _lt.position = edgePoints[1];
        _rt.position = edgePoints[2];
        _rb.position = edgePoints[3];
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