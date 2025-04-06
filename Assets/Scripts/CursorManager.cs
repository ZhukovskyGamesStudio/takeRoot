using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
    public static CursorManager Instance;

    [SerializeField]
    private Texture2D _search, _move, _attack;

    [SerializeField]
    private Texture2D _undecidedCursor, _roboCursor, _plantsCursor;

    [SerializeField]
    private List<Texture2D> _wakeupAnimation;

    private Coroutine _cursorAnimation;
    private Texture2D _normal;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void SetCursorByRace(Race race) {
        if (race == Race.None) {
            _normal = _undecidedCursor;
        } else {
            _normal = race == Race.Robots ? _roboCursor : _plantsCursor;
        }

        ChangeCursor(CursorType.Normal);
    }

    public static void ChangeCursor(CursorType type) {
        Instance.StopAnimation();
        Texture2D newTexture;
        if (type == CursorType.Wakeup) {
            Instance.StartAnimation();
            return;
        }

        switch (type) {
            case CursorType.Normal:
                newTexture = Instance._normal;
                break;
            case CursorType.Search:
                newTexture = Instance._search;
                break;
            case CursorType.TacticalMove:
                newTexture = Instance._move;
                break;
            case CursorType.TacticalAttack:
                newTexture = Instance._attack;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        Cursor.SetCursor(newTexture, Vector2.zero, CursorMode.Auto);
    }

    private void StartAnimation() {
        _cursorAnimation = StartCoroutine(CursorAnimation(_wakeupAnimation, Vector2.one * 5, 0.25f));
    }

    private IEnumerator CursorAnimation(List<Texture2D> textures, Vector2 shift, float delay) {
        int cur = 0;
        while (true) {
            Cursor.SetCursor(textures[cur], shift, CursorMode.Auto);
            cur++;
            cur %= textures.Count;
            yield return new WaitForSeconds(delay);
        }
    }

    private void StopAnimation() {
        if (_cursorAnimation != null) {
            StopCoroutine(_cursorAnimation);
        }
    }
}

public enum CursorType {
    Normal,
    Search,
    Wakeup,
    TacticalMove,
    TacticalAttack
}