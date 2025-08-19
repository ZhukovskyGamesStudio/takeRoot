using System.Collections.Generic;
using UnityEngine;

public class PeashooterView : MonoBehaviour {
    [SerializeField]
    private List<SpriteRenderer> _bullets;

    private int _bulletIndex;

    public void DoShootView() {
        if(_bulletIndex >= _bullets.Count) {
            return;
        }
        _bullets[_bulletIndex].enabled = false;
        _bulletIndex++;
    }

    public void DoReloadView() {
        _bulletIndex = 0;
        foreach (var VARIABLE in _bullets) {
            VARIABLE.enabled = true;
        }
    }
}