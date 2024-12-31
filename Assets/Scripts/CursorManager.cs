using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
    public static CursorManager Instance;

    [SerializeField]
    private Texture2D _normal, _search;

    [SerializeField]
    private List<Texture2D> _wakeupAnimation;

    private Coroutine _cursorAnimation;

    private void Awake() {
        Instance = this;
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
    Wakeup
}