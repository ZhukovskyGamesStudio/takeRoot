using UnityEngine;
using UnityEngine.UI;

public class GameSpeed : MonoBehaviour
{
    [SerializeField]
    private Image _normalspeed,_doublespeed,_triplespeed,_pause;
    
    public Sprite _normalspeedact, _doublespeedact, _triplespeedact, _pauseact;
    public void NormalSpeed() {
        _normalspeed.sprite = _normalspeedact;
    }

    public void DoubleSpeed() {
        _doublespeed.sprite = _doublespeedact;
    }

    public void TriplesSpeed() {
        _triplespeed.sprite = _triplespeedact;
    }

    public void Pause() {
        _pause.sprite = _pauseact;
    }
}

